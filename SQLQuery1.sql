CREATE TABLE users2 (
	name VARCHAR(100) NOT NULL,
	email VARCHAR(150),
	password VARCHAR(200) NOT NULL,
	borrowedbook VARCHAR(500),
	reservedbook VARCHAR(500),
	created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO users2 (name,email,password,borrowedbook,reservedbook)
VALUES
('librarian','','123','',''),
('jeremy','','123','',''),
('james','','123','',''),
('richard','','123','','')