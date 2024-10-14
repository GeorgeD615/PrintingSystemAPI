CREATE DATABASE PrintingSystem;
GO

USE PrintingSystem;
GO

-- Таблица для типов подключения
CREATE TABLE ConnectionType (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL
);

-- Таблица устройств печати
CREATE TABLE PrintingDevice (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    connection_type_id UNIQUEIDENTIFIER,
    FOREIGN KEY (connection_type_id) REFERENCES ConnectionType(id)
);

-- Таблица для MAC-адресов
CREATE TABLE MACaddresses (
    id CHAR(17) PRIMARY KEY,
    printing_device_id UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (printing_device_id) REFERENCES PrintingDevice(id)
);

-- Таблица филиалов
CREATE TABLE Office (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL
);

-- Таблица инсталляций устройств
CREATE TABLE Installation (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    installation_order_number INT NULL,
    is_default BIT NOT NULL,
    printing_device_id UNIQUEIDENTIFIER NOT NULL,
    office_id UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (printing_device_id) REFERENCES PrintingDevice(id),
    FOREIGN KEY (office_id) REFERENCES Office(id),
);

CREATE UNIQUE INDEX UQ_Installation_Default 
ON Installation (office_id) 
WHERE is_default = 1;

-- Таблица сотрудников
CREATE TABLE Employee (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    office_id UNIQUEIDENTIFIER NOT NULL,
    name NVARCHAR(255) NOT NULL,
    FOREIGN KEY (office_id) REFERENCES Office(id)
);

-- Таблица статусов печати
CREATE TABLE SeccionStatus (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(50) NOT NULL
);

-- Таблица сеансов печати
CREATE TABLE Session (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    task_name NVARCHAR(255) NOT NULL,
    employee_id UNIQUEIDENTIFIER NOT NULL,
    installation_id UNIQUEIDENTIFIER NOT NULL,
    number_of_pages INT NOT NULL,
    status_id UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES Employee(id),
    FOREIGN KEY (installation_id) REFERENCES Installation(id) ON DELETE CASCADE,
    FOREIGN KEY (status_id) REFERENCES SeccionStatus(id)
);

INSERT INTO ConnectionType (name) VALUES (N'Локальное'), (N'Сетевое');
INSERT INTO SeccionStatus (name) VALUES (N'Успех'), (N'Неудача');

INSERT INTO Office (name) VALUES (N'Тридевятое царство'), (N'Дремучий Лес'), (N'Луна');

INSERT INTO Employee (name, office_id) VALUES 
(N'Царь', (SELECT id FROM Office WHERE name = N'Тридевятое царство')),
(N'Яга', (SELECT id FROM Office WHERE name = N'Дремучий Лес')),
(N'Копатыч', (SELECT id FROM Office WHERE name = N'Луна')),
(N'Добрыня', (SELECT id FROM Office WHERE name = N'Тридевятое царство')),
(N'Кощей', (SELECT id FROM Office WHERE name = N'Луна')),
(N'Лосяш', (SELECT id FROM Office WHERE name = N'Луна'));

INSERT INTO PrintingDevice (name, connection_type_id) VALUES 
(N'Папирус', (SELECT id FROM ConnectionType WHERE name = N'Локальное')),
(N'Бумага ', (SELECT id FROM ConnectionType WHERE name = N'Локальное')),
(N'Камень', (SELECT id FROM ConnectionType WHERE name = N'Сетевое'));

INSERT INTO Installation (name, installation_order_number, is_default, printing_device_id, office_id) VALUES 
(N'Дворец', 1, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'Папирус'),
       (SELECT id FROM Office WHERE name = N'Тридевятое царство')),
(N'Конюшни', 2, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'Бумага'),
       (SELECT id FROM Office WHERE name = N'Тридевятое царство')),
(N'Оружейная', 3, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'Бумага'),
       (SELECT id FROM Office WHERE name = N'Тридевятое царство')),
(N'Кратер', 1, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'Камень'),
       (SELECT id FROM Office WHERE name = N'Луна')),
(N'Избушка', 3, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'Бумага'),
       (SELECT id FROM Office WHERE name = N'Дремучий Лес')),
(N'Топи', 2, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'Папирус'),
       (SELECT id FROM Office WHERE name = N'Дремучий Лес'));
GO
