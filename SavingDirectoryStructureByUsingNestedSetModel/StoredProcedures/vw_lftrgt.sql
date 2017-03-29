CREATE VIEW vw_lftrgt AS select DirectoryTreeMap.lft AS lft from DirectoryTreeMap union select DirectoryTreeMap.rgt AS rgt from DirectoryTreeMap;
