-- Сотрудники компании
SELECT Employee.name AS Сотрудник, Office.name AS Филиал FROM
	Employee JOIN Office ON Employee.office_id = Office.id

