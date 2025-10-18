START TRANSACTION;
CREATE TABLE `Usuaris` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Username` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
    `FullName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Role` varchar(20) CHARACTER SET utf8mb4 NOT NULL DEFAULT 'User',
    `CreatedAt` datetime(6) NOT NULL,
    `IsEnabled` tinyint(1) NOT NULL DEFAULT TRUE,
    CONSTRAINT `PK_Usuaris` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `Usuaris` (`Id`, `CreatedAt`, `Email`, `FullName`, `IsEnabled`, `PasswordHash`, `Role`, `Username`)
VALUES (1, TIMESTAMP '2025-01-01 00:00:00', 'admin@comandesjdsr.com', 'Administrador del Sistema', TRUE, '$2a$12$LKw3pQ7vXZ8nYx9zK.J8Vu3EYqH7wZGJ2YdF5mXK8pHv1lN3oQ6qm', 'Administrator', 'administrador'),
(2, TIMESTAMP '2025-01-01 00:00:00', 'usuari@comandesjdsr.com', 'Usuari Estàndard', TRUE, '$2a$12$LKw3pQ7vXZ8nYx9zK.J8Vu3EYqH7wZGJ2YdF5mXK8pHv1lN3oQ6qm', 'User', 'usuari');

CREATE UNIQUE INDEX `IX_Usuaris_Email` ON `Usuaris` (`Email`);

CREATE UNIQUE INDEX `IX_Usuaris_Username` ON `Usuaris` (`Username`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251018155434_AddUsuarisTable', '9.0.9');

COMMIT;

