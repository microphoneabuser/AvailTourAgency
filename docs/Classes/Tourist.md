# Класс Employee
    
Данный класс наследуется от класса [Person](./Person.md "Person"). 

## Атрибуты

* ID: int - идентификатор
* FIO: string - Фамилия Имя Отчество
* Passport: string - серия и номер паспорта
* PhoneNumber: string - номер телефона телефон
* Deleted: bool - индикатор удаления
* DateOfBirth: DateTime - дата рождения
* DocumentType: [DocumentType](./DocumentType.md "enum интерфейс DocumentType") - вид документа, удостоверяющего личность.
* DocumentData: string - серия и номер документа, удостоверяющего личность.