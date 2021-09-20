# Класс TourDatesPrice

## Атрибуты

- ID: int - идентификатор
- Tour: [Tour](./Tour.md "Tour") - тур, вариацией которого является инстанс данного класса
- FlyDateThere: DateTime - дата вылета туда
- FlyDateBack: DateTime - дата вылета обратно
- Length: int - длительность тура
- Price: decimal - стоимость
- Airline: string - авиакомпания
- FlightClass: [FlightClass](./FlightClass.md "enum интерфейс FlightClass") - класс авиарейса
- Luggage: string - информация о перевозке багажа
- Meals: string - информация о питании
- Quantity: int - количество
- Deleted: bool - индикатор удаления