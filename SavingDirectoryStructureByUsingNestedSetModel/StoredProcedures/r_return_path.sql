-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------


CREATE PROCEDURE r_return_path(

  @pnode_id INT,
  @plang CHAR(2)

) as
BEGIN

  SELECT p.node_id, name
    FROM tree_map AS n, tree_map AS p
    LEFT JOIN tree_content AS tc ON p.node_id = tc.node_id 
   WHERE n.lft BETWEEN p.lft AND p.rgt
     AND n.node_id = @pnode_id
     AND lang = @plang
   ORDER BY p.lft;

END