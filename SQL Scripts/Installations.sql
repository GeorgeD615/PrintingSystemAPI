-- ������������� ��������� � �������� ��������
SELECT Installation.name AS �����������, 
	   Office.name AS ������, 
	   Installation.installation_order_number AS [���������� �����], 
	   CASE 
           WHEN Installation.is_default = 1 THEN N'��' 
           ELSE N'���' 
       END AS [�� ���������], 
	   PrintingDevice.name AS [���������� ������] 
FROM PrintingDevice JOIN Installation ON PrintingDevice.id = Installation.printing_device_id
				    JOIN Office ON Installation.office_id = Office.id