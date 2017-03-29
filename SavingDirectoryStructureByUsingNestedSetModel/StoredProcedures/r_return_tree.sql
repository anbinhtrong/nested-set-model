-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------


CREATE PROCEDURE r_return_tree(

  @pedited INT,
  @plang CHAR(2)

) AS
BEGIN	
  IF @pedited IS NULL 
  Begin
    SELECT n.node_id,
      CONCAT( REPEAT(' . . ', COUNT(CAST(p.node_id AS CHAR)) - 1), 
      (SELECT name FROM tree_content WHERE node_id = n.node_id AND lang = @plang)) AS name
    FROM tree_map AS n, tree_map AS p
    WHERE (n.lft BETWEEN p.lft AND p.rgt)
    GROUP BY node_id
    ORDER BY n.lft;
	End
  ELSE
  Begin
    SELECT n.node_id,
      CONCAT( REPEAT(' . . ', COUNT(CAST(p.node_id AS CHAR)) - 1), 
      (SELECT name FROM tree_content WHERE node_id = n.node_id AND lang = @plang)) AS name
    FROM tree_map AS n, tree_map AS p
    WHERE (n.lft BETWEEN p.lft AND p.rgt) AND n.node_id != @pedited
    GROUP BY node_id
    ORDER BY n.lft;

  END
       
END