-- ���������� ��� ������ �� ������
SELECT PrintingDevice.name AS ������������, ConnectionType.name AS [��� �����������] FROM 
	PrintingDevice JOIN ConnectionType ON PrintingDevice.connection_type_id = ConnectionType.id