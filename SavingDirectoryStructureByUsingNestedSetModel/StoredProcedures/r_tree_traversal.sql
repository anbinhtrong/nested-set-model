CREATE PROCEDURE r_tree_traversal(
  @ptask_type VARCHAR(10),
  @pnode_id INT,
  @pparent_id INT
) AS
BEGIN
  DECLARE @new_lft int, 
  @new_rgt int, @width int, @has_leafs int, @superior int, @superior_parent int, @old_lft int, 
  @old_rgt int, @parent_rgt int, @subtree_size int;

  IF( @ptask_type = 'insert'  )
   Begin
        SELECT @new_lft = rgt FROM DirectoryTreeMap WHERE nodeId = @pparent_id;
			UPDATE DirectoryTreeMap SET rgt = rgt + 2 WHERE rgt >= @new_lft;
			UPDATE DirectoryTreeMap SET lft = lft + 2 WHERE lft > @new_lft;
        INSERT INTO DirectoryTreeMap (lft, rgt, parentId) VALUES (@new_lft, (@new_lft + 1), @pparent_id);
		SELECT SCOPE_IDENTITY();
	END
    ELSE
    IF(@ptask_type = 'delete')
	BEGIN
    
        SELECT @new_lft = Lft, @new_rgt = rgt, @has_leafs = (rgt - lft), @width = (rgt - lft + 1), @superior_parent=parentId 
		  FROM DirectoryTreeMap WHERE nodeId = @pnode_id;

		DELETE FROM dbo.TreeContent WHERE nodeId = @pnode_id;

        IF (@has_leafs = 1) 
		BEGIN
          DELETE FROM DirectoryTreeMap WHERE lft BETWEEN @new_lft AND @new_rgt;
          UPDATE DirectoryTreeMap SET rgt = rgt - @width WHERE rgt > @new_rgt;
          UPDATE DirectoryTreeMap SET lft = lft - @width WHERE lft > @new_rgt;
		  END          
        ELSE
		Begin
          DELETE FROM DirectoryTreeMap WHERE lft = @new_lft;
          UPDATE DirectoryTreeMap SET rgt = rgt - 1, lft = lft - 1, parentId = @superior_parent 
		   WHERE lft BETWEEN @new_lft AND @new_rgt;
          UPDATE DirectoryTreeMap SET rgt = rgt - 2 WHERE rgt > @new_rgt;
          UPDATE DirectoryTreeMap SET lft = lft - 2 WHERE lft > @new_rgt;
        END;

	END
    
	ELSE
	--move
	IF(@ptask_type = 'move')
    Begin
    
    
		IF (@pnode_id != @pparent_id) 
		BEGIN
			IF OBJECT_ID('working_DirectoryTreeMap') IS NOT NULL
			  /*Then it exists*/
			  DROP TABLE working_DirectoryTreeMap

				CREATE TABLE working_DirectoryTreeMap
					(
					  nodeId int IDENTITY(1,1) PRIMARY KEY,
					  lft int DEFAULT NULL,
					  rgt int DEFAULT NULL,
					  parentId int NOT NULL
					);			
        
			-- put subtree into temporary table
			INSERT INTO working_DirectoryTreeMap (node_id, lft, rgt, parent_id)
				 SELECT t1.nodeId, 
						(t1.lft - (SELECT MIN(lft) FROM DirectoryTreeMap WHERE nodeId = @pnode_id)) AS lft,
						(t1.rgt - (SELECT MIN(lft) FROM DirectoryTreeMap WHERE nodeId = @pnode_id)) AS rgt,
						t1.parentId
				   FROM DirectoryTreeMap AS t1, DirectoryTreeMap AS t2
				  WHERE t1.lft BETWEEN t2.lft AND t2.rgt 
					AND t2.nodeId = @pnode_id;

			DELETE FROM DirectoryTreeMap WHERE nodeId IN (SELECT nodeId FROM working_DirectoryTreeMap);

			SELECT rgt INTO parent_rgt FROM DirectoryTreeMap WHERE nodeId = @pparent_id;
			SET @subtree_size = (SELECT (MAX(rgt) + 1) FROM working_DirectoryTreeMap);
		
			-- make a gap in the tree
			UPDATE DirectoryTreeMap
			  SET lft = (CASE WHEN lft > @parent_rgt THEN lft + @subtree_size ELSE lft END),
				  rgt = (CASE WHEN rgt >= @parent_rgt THEN rgt + @subtree_size ELSE rgt END)
			WHERE rgt >= @parent_rgt;

			INSERT INTO DirectoryTreeMap (node_id, lft, rgt, parent_id)
				 SELECT nodeId, lft + parent_rgt, rgt + parent_rgt, parent_id
				   FROM working_DirectoryTreeMap;
        
			-- close gaps in tree
			UPDATE DirectoryTreeMap
			   SET lft = (SELECT COUNT(*) FROM vw_lftrgt AS v WHERE v.lft <= DirectoryTreeMap.lft),
				   rgt = (SELECT COUNT(*) FROM vw_lftrgt AS v WHERE v.lft <= DirectoryTreeMap.rgt);
        
			DELETE FROM working_DirectoryTreeMap;
			UPDATE DirectoryTreeMap SET parentId = @pparent_id WHERE nodeId = @pnode_id;
		END
	END
    else
    --WHEN @ptask_type = 'order' THEN
	Begin
        SELECT @old_lft = lft, @old_rgt = rgt, @width = (rgt - lft + 1), @superior = parentId 
          FROM DirectoryTreeMap WHERE nodeId = @pnode_id;

        -- is parent 
        SELECT @superior_parent = parentId FROM DirectoryTreeMap WHERE nodeId = @pparent_id;

        IF (@superior = @superior_parent)
		Begin
          SELECT @new_lft =(rgt + 1) FROM DirectoryTreeMap WHERE nodeId = @pparent_id;
		END
        ELSE
		Begin
          SELECT @new_lft =(lft + 1)  FROM DirectoryTreeMap WHERE nodeId = @pparent_id;
        END

	    IF (@new_lft != @old_lft)
		BEGIN
			IF OBJECT_ID('working_DirectoryTreeMap') IS NOT NULL
				  /*Then it exists*/
				  DROP TABLE working_DirectoryTreeMap

					CREATE TABLE working_DirectoryTreeMap
						(
						  nodeId int IDENTITY(1,1) PRIMARY KEY,
						  lft int DEFAULT NULL,
						  rgt int DEFAULT NULL,
						  parentId int NOT NULL
						);			        		  
		End
	     INSERT INTO working_DirectoryTreeMap (node_id, lft, rgt, parent_id)
            SELECT t1.nodeId,
			  	   (t1.lft - (SELECT MIN(lft) FROM DirectoryTreeMap WHERE nodeId = @pnode_id)) AS lft,
				   (t1.rgt - (SELECT MIN(lft) FROM DirectoryTreeMap WHERE nodeId = @pnode_id)) AS rgt,
				   t1.parentId
			  FROM DirectoryTreeMap AS t1, DirectoryTreeMap AS t2
			 WHERE t1.lft BETWEEN t2.lft AND t2.rgt AND t2.nodeId = @pnode_id;
            
       DELETE FROM DirectoryTreeMap WHERE nodeId IN (SELECT nodeId FROM working_DirectoryTreeMap);

       IF(@new_lft < @old_lft)  -- move up
	   Begin
          UPDATE DirectoryTreeMap SET lft = lft + @width WHERE lft >= @new_lft AND lft < @old_lft;
          UPDATE DirectoryTreeMap SET rgt = rgt + @width WHERE rgt > @new_lft AND rgt < @old_rgt;
          UPDATE working_DirectoryTreeMap SET lft = @new_lft + lft, rgt = @new_lft + rgt;
       END

       IF(@new_lft > @old_lft) 
	   BEGIN -- move down
            UPDATE DirectoryTreeMap SET lft = lft - @width WHERE lft > @old_lft AND lft < @new_lft;
            UPDATE DirectoryTreeMap SET rgt = rgt - @width WHERE rgt > @old_rgt AND rgt < @new_lft;
            UPDATE working_DirectoryTreeMap SET lft = (@new_lft - @width) + lft, rgt = (@new_lft - @width) + rgt;
       END

       INSERT INTO DirectoryTreeMap (node_id, lft, rgt, parent_id)
            SELECT nodeId, lft, rgt, parent_id
              FROM working_DirectoryTreeMap;
            
       DELETE FROM working_DirectoryTreeMap;
	   END
  END

--END