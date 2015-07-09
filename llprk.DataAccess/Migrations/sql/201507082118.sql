update Pictures
set ProductId = (select top 1 ProductId
				from Product_Picture 
				where Product_Picture.PictureId = Pictures.Id),
Pos = ISNULL((select top 1 Pos
				from Product_Picture 
				where Product_Picture.PictureId = Pictures.Id), 0)
