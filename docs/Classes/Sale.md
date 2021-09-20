# Класс Sale

## Атрибуты

- ID: int - идентификатор
- SaleDate: DateTime - дата продажи
- Client: [Client](./Client.md "Client") - клиент, по отношению к которому совершена продажа
- Tour: [Tour](./Tour.md "Tour") - тур
- Hotel: [Hotel](./Hotel.md "Hotel") - отель
- HotelRoom: [HotelRoom](./HotelRoom.md "HotelRoom") - номер отеля
- TourDatesPrice: [TourDatesPrice](./TourDatesPrice.md "TourDatesPrice") - вариация тура
- Price:  decimal - стоимость продажи в целом
- PaymentMethod: string - способ оплаты
- Employee: [Employee](./Employee.md "Employee") - сотрудник, которым совершена продажа
- Deleted: bool - индикатор удаления