CREATE PROCEDURE r_return_subtree
    (
      @pnode_id INT ,
      @plang CHAR(2)
    )
AS
    BEGIN

        SELECT  node.NodeId ,
                ( COUNT(parent.NodeId) - ( sub_tree.depth + 1 ) ) AS depth ,
                ( SELECT    name
                  FROM      tree_content
                  WHERE     node_id = node.node_id
                            AND lang = @plang
                ) AS name
        FROM    DirectoryTreeMap AS node ,
                DirectoryTreeMap AS parent ,
                DirectoryTreeMap AS sub_parent ,
                ( SELECT    node.NodeId ,
                            ( COUNT(parent.NodeId) - 1 ) AS depth
                  FROM      DirectoryTreeMap AS node ,
                            DirectoryTreeMap AS parent
                  WHERE     node.Lft BETWEEN parent.Lft AND parent.Rgt
                            AND node.NodeId = @pnode_id
                  GROUP BY  node.NodeId
                  --ORDER BY  node.Lft
                ) AS sub_tree
        WHERE   node.Lft BETWEEN parent.Lft AND parent.Rgt
                AND node.Lft BETWEEN sub_parent.Lft
                             AND     sub_parent.Rgt
                AND sub_parent.NodeId = sub_tree.NodeId
        GROUP BY node.NodeId
        ORDER BY node.Lft;

    END