# Интерфейс ITourist

Интерфейс работает с методами класса [Tourist](../Classes/Tourist.md).

## Методы интерфейса

- **Add** (ID: int): Result – функция, добавляющая туриста. Возвращает инстанс класса Result с результатом операции и доп. информацией при ошибке;
- **Del** (ID: int): Result – функция, удаляющая туриста. Возвращает инстанс класса Result с результатом операции и доп. информацией при ошибке;
- **Edit** (ID: int): Result – функция, редактирующая туриста. Возвращает инстанс класса Result с ре-зультатом операции и доп. информацией при ошибке;
- **GetSales** (ID: int): List< Sale > – функция, которая возвращает список продаж, частью которой является определенный турист. Параметр ID – идентификатор туриста, про-дажи которого должна вывести функция;
- **GetAll** (sorting: string, AskOrDesk: string, filterA: Tourist, filterB: Tourist, count: int, page: int): List< Tourist > – функция, возвращающая список туристов с заданными па-раметрами. Параметры:  
    -	*sorting*: string – отвечает, по какому полю будет сортироваться список:
    -	*AskOrDesk*: string – отвечает, по возрастанию или убыванию будут сортиро-ваться элементы;
    -	*filterA*: Tourist – отвечает за фильтрацию, включает в себя левую границу интер-вала значений фильтра;
    -	*filter*: Tourist – отвечает за фильтрацию, включает в себя правую границу интер-вала значений фильтра; 
    -	*count*: int – отвечает, сколько элементов необходимо показать;
    -	*page*: int – отвечает, с какой страницы начинать поиск элементов. 