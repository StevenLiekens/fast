CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200501231518_CreateDatabase') THEN

    CREATE TABLE `Items` (
        `ItemId` int NOT NULL AUTO_INCREMENT,
        `Name` longtext CHARACTER SET utf8mb4 NULL,
        `Buy` int NOT NULL,
        `Sell` int NOT NULL,
        CONSTRAINT `PK_Items` PRIMARY KEY (`ItemId`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200501231518_CreateDatabase') THEN

    CREATE TABLE `SelectionContainers` (
        `SelectionContainerId` int NOT NULL AUTO_INCREMENT,
        `Name` longtext CHARACTER SET utf8mb4 NULL,
        `Buy` int NOT NULL,
        `Sell` int NOT NULL,
        CONSTRAINT `PK_SelectionContainers` PRIMARY KEY (`SelectionContainerId`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200501231518_CreateDatabase') THEN

    CREATE TABLE `SelectionContainerItems` (
        `SelectionContainerId` int NOT NULL,
        `ItemId` int NOT NULL,
        `Guaranteed` tinyint(1) NOT NULL,
        `Amount` int NOT NULL,
        CONSTRAINT `PK_SelectionContainerItems` PRIMARY KEY (`SelectionContainerId`, `ItemId`),
        CONSTRAINT `FK_SelectionContainerItems_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`ItemId`) ON DELETE CASCADE,
        CONSTRAINT `FK_SelectionContainerItems_SelectionContainers_SelectionContain~` FOREIGN KEY (`SelectionContainerId`) REFERENCES `SelectionContainers` (`SelectionContainerId`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200501231518_CreateDatabase') THEN

    CREATE INDEX `IX_SelectionContainerItems_ItemId` ON `SelectionContainerItems` (`ItemId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200501231518_CreateDatabase') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20200501231518_CreateDatabase', '3.1.3');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

