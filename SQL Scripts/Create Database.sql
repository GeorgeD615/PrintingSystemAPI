CREATE DATABASE PrintingSystem;
GO

USE PrintingSystem;
GO

-- ������� ��� ����� �����������
CREATE TABLE ConnectionType (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL
);

-- ������� ��������� ������
CREATE TABLE PrintingDevice (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    connection_type_id UNIQUEIDENTIFIER,
    FOREIGN KEY (connection_type_id) REFERENCES ConnectionType(id)
);

-- ������� ��� MAC-�������
CREATE TABLE MACaddresses (
    id CHAR(17) PRIMARY KEY,
    printing_device_id UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (printing_device_id) REFERENCES PrintingDevice(id)
);

-- ������� ��������
CREATE TABLE Office (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL
);

-- ������� ����������� ���������
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

-- ������� �����������
CREATE TABLE Employee (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    office_id UNIQUEIDENTIFIER NOT NULL,
    name NVARCHAR(255) NOT NULL,
    FOREIGN KEY (office_id) REFERENCES Office(id)
);

-- ������� �������� ������
CREATE TABLE SeccionStatus (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(50) NOT NULL
);

-- ������� ������� ������
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

INSERT INTO ConnectionType (name) VALUES (N'���������'), (N'�������');
INSERT INTO SeccionStatus (name) VALUES (N'�����'), (N'�������');

INSERT INTO Office (name) VALUES (N'���������� �������'), (N'�������� ���'), (N'����');

INSERT INTO Employee (name, office_id) VALUES 
(N'����', (SELECT id FROM Office WHERE name = N'���������� �������')),
(N'���', (SELECT id FROM Office WHERE name = N'�������� ���')),
(N'�������', (SELECT id FROM Office WHERE name = N'����')),
(N'�������', (SELECT id FROM Office WHERE name = N'���������� �������')),
(N'�����', (SELECT id FROM Office WHERE name = N'����')),
(N'�����', (SELECT id FROM Office WHERE name = N'����'));

INSERT INTO PrintingDevice (name, connection_type_id) VALUES 
(N'�������', (SELECT id FROM ConnectionType WHERE name = N'���������')),
(N'������ ', (SELECT id FROM ConnectionType WHERE name = N'���������')),
(N'������', (SELECT id FROM ConnectionType WHERE name = N'�������'));

INSERT INTO Installation (name, installation_order_number, is_default, printing_device_id, office_id) VALUES 
(N'������', 1, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'�������'),
       (SELECT id FROM Office WHERE name = N'���������� �������')),
(N'�������', 2, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'������'),
       (SELECT id FROM Office WHERE name = N'���������� �������')),
(N'���������', 3, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'������'),
       (SELECT id FROM Office WHERE name = N'���������� �������')),
(N'������', 1, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'������'),
       (SELECT id FROM Office WHERE name = N'����')),
(N'�������', 3, 0, 
       (SELECT id FROM PrintingDevice WHERE name = N'������'),
       (SELECT id FROM Office WHERE name = N'�������� ���')),
(N'����', 2, 1, 
       (SELECT id FROM PrintingDevice WHERE name = N'�������'),
       (SELECT id FROM Office WHERE name = N'�������� ���'));
GO
