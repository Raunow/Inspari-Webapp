declare @PageSize int = 25;
declare @PageNumber int = 1;

SELECT * FROM [Todo] td
Order by td.Creation
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY

