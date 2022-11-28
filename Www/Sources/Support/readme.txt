step 1: check sql about exist fulltextsearch in ms sql 2008r2 with the query above:
select SERVERPROPERTY('isfulltextinstalled')
= 1 is exist
= 0 when you need to install it in your ms sql 
reference: http://kieutrongkhanh.net/index.php/servlet-jsp/107-fulltext-index-search-sqlserver
