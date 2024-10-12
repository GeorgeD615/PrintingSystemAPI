-- Устройства для вывода на печать
SELECT PrintingDevice.name AS Наименование, ConnectionType.name AS [Тип подключения] FROM 
	PrintingDevice JOIN ConnectionType ON PrintingDevice.connection_type_id = ConnectionType.id