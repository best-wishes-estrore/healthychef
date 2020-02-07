IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[hccPrograms]') 
         AND name = 'DisplayOnWebsite'
)
BEGIN
	ALTER TABLE [dbo].[hccPrograms] ADD DisplayOnWebsite bit not null DEFAULT(1)
END
GO