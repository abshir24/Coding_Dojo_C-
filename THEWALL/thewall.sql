-- select X.UsersId, Y.MessagesId, A.CommentId,A.Comment from Users X, Messages Y,Comments A;-- 

-- SELECT X.UsersId, A.Comment FROM Users X, Messages Y, Comments A where X.UsersId = Y.UsersId and Y.MessagesId = A.MessagesId;
SELECT X.UsersId, Y.Messages, X.FirstName, Y.Created_At, Y.MessagesId,A.CommentId,A.Comment FROM Users X, Messages Y, Comments A where X.UsersId = Y.UsersId and Y.MessagesId = A.MessagesId;