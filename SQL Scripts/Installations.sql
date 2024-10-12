-- Использование устройств в филиалах компании
SELECT Installation.name AS Инсталляция, 
	   Office.name AS Филиал, 
	   Installation.installation_order_number AS [Порядковый номер], 
	   CASE 
           WHEN Installation.is_default = 1 THEN N'Да' 
           ELSE N'Нет' 
       END AS [По умолчанию], 
	   PrintingDevice.name AS [Устройство печати] 
FROM PrintingDevice JOIN Installation ON PrintingDevice.id = Installation.printing_device_id
				    JOIN Office ON Installation.office_id = Office.id