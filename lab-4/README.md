# Сборщик мусора. Генетический алгоритм

## Этап 1

### Написание минимальной работающей программы

Был использован [шаблон из организации](https://github.com/is-tech-y24-1/GeneticAlgoTemplate).

Задача - найти маршрут из одной точки в другую, избегая препятствия.

Для этого помимо сущностей, представленных в шаблоне, была создана новая - траектория *(Trajectory)*.
Она представляла из себя список векторов - перемещений точки на каждой итерации.

```csharp
namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private List<Point> _vectors;
    private Point _result;

    public Point Result => _result;

    public Trajectory(Trajectory? trajectory = null)
    {
        if (trajectory is null)
        {
            _vectors = new List<Point>();
            _result = new Point(0, 0);
        }
        else
        {
            _vectors = trajectory._vectors.ToList();
            _result = trajectory._result;
        }
        
    }

    public void AddVector(Point point)
    {
        _vectors.Add(point);
        _result.X += point.X;
        _result.Y += point.Y;
    }
}
```

В новом решении каждое перемещение для каждой из точек считается случайным образом с условием, что точка не выйдет за границы холста и не попадет в препятствие.

```csharp
public Task<IterationResult> ExecuteIterationAsync()
{
    if (++_iterationCounter > _iterationLimit)
        return Task.FromResult(IterationResult.SolutionCannotBeFound);
        
        
    _trajectories = _trajectories.OrderBy(t => _math.Fitness(t.Result)).ToArray();

    if (_math.Fitness(_trajectories[0].Result) < _fitnessAccuracy)
        return Task.FromResult(IterationResult.SolutionFound);

        
    for (var i = 0; i < _trajectories.Length * _partOfBadTrajectoriesToReplace; i++)
    {
        _trajectories[^(i + 1)] = new Trajectory(_trajectories[i]);
    }

    for (var i = 1; i < _trajectories.Length; i++)
    {
        _math.Mutate(_trajectories[i]);
    }

    return Task.FromResult(IterationResult.IterationFinished);
 }
```

Обнаружив, что отрисовка круглых препятствий не зависит от их радиуса, исправим это. Теперь фактический радиус круга соответствует толщине его обводки.

![Картинка из UI с найденным решением](./img/display-1.png)

## Этап 2

### Анализ работы программы с помощью dotTrace и dotMemory

Для анализа производительности генетического алгоритма напишем реализацию его запуска без UI.

Результат бенчмаркинга:

```
|  Method | PointsCount | Epsilon |        Mean |     Error |    StdDev |
|-------- |------------ |-------- |------------:|----------:|----------:|
| Execute |        5000 |    0.01 |    81.04 ms |  17.22 ms |  9.008 ms |
| Execute |        5000 |     0.1 |    60.07 ms |  14.81 ms |  9.795 ms |
| Execute |       50000 |    0.01 | 1,081.25 ms | 128.43 ms | 67.171 ms |
| Execute |       50000 |     0.1 |   793.29 ms |  74.31 ms | 49.153 ms |

```

![Результат первого запуска в dotTrace](./img/dottrace-1.png)

![Функция Mutate крупным планом при первом запуске в dotTrace](./img/dottrace-mutate-1.png)

Видим, что более 40% времени занимает функция ToArray() - очень плохо, надо оптимизировать.

Возможное решение - использовать такие структуры, чтобы не требовалось постоянное копирование массива.

![Результат первого запуска в dotMemory](./img/dotmemory-1.png)

Видим, что сборщик мусора часто работает - очень плохо, надо фиксить.

Возможное решение - использование таких структур, чтобы не требовалось пересоздавать долгоживущие объекты.

## Этап 3

### Оптимизация решения

#### Проблема 1

Одна из проблем заключалась в том, что при сортировке массива траекторий приходилось вызывать метод ToArray(), что занимало и время, и требовало много памяти.
Решение - хранить траектории в списке. Список траекторий постоянен, поэтому проблемы копирования при увеличении листа не будет.

```csharp
private List<Trajectory> _trajectories;
```

Мы заранее знаем количество траекторий. И, исходя из задачи, предполагаем, что оно будет достаточно велико.
Поэтому создавая пустой список и добавляя к нему траектории, мы будем сталкиваться с нехваткой _Capacity_, из-за чего списки будут пересоздаваться. Чтобы избежать этого, сразу создадим список нужного нам размера.

```csharp
_trajectories = new List<Trajectory>(_pointCount);

for (var i = 0; i < _pointCount; i++)
{
    _trajectories.Add(new Trajectory(_iterationLimit));
}
```

![Результат в dotTrace после замены массива траекторий на список](./img/dottrace-2.png)

Теперь много времени уходит на подсчет фитнес-функции. Исправим это, считая фитнес для каждой траектории только один раз за итерацию и храня его полем.

```csharp
foreach (var trajectory in _trajectories)
{
    trajectory.Fitness = _math.Fitness(trajectory.Result);
}
```

![Результат в dotTrace после сохранения фитнеса](./img/dottrace-3.png)

Стало лучше!

#### Проблема 2

Теперь будем избавляться от частого создания долгоживущих элементов.

Сейчас точки траекторий хранятся в списке. На каждой итерации к списку добавляется новая точка, списки не влезают в рамки, из-за чего регулярно создаются новые.

Как от этого избавиться? На каждой итерации мы добавляем к траектории по одной точке.
Зная заранее максимальное количество итераций, мы знаем также и максимальное количество точек в траектории,
значит, можем хранить точки в массиве.

```csharp
namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private readonly Point[] _points;
    private int _pointCount;

    public Point Result => _points[_pointCount - 1];
    public double Fitness { get; set; }

    public int Length => _pointCount;

    public Point[] Points => _points;

    public Trajectory(int maxLength)
    {
        _points = new Point[maxLength];
        _points[0] = new Point(0, 0);
        _pointCount = 1;
    }

    public void AddPoint(Point point)
    {
        _points[_pointCount] = point;
        _pointCount++;
    }
}
```

В итоге имеем код для итерации:

```csharp
public Task<IterationResult> ExecuteIterationAsync()
{
    // check iteration limit
    if (++_iterationCounter > _iterationLimit)
        return Task.FromResult(IterationResult.SolutionCannotBeFound);

    // set fitness
    foreach (var trajectory in _trajectories)
    {
        trajectory.Fitness = _math.Fitness(trajectory.Result);
    }


    // get best element
    _trajectories.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
    if (_trajectories[0].Fitness < _fitnessAccuracy)
        return Task.FromResult(IterationResult.SolutionFound);
        
        
    // replace bad elements with good
    for (var i = 0; i < _trajectories.Count * _partOfBadTrajectoriesToReplace; i++)
    {
        for (var j = 0; j < _trajectories[i].Length; j++)
            _trajectories[^(i + 1)].Points[j] = _trajectories[i].Points[j];
    }
        
    // mutate
    for (var i = 1; i < _trajectories.Count; i++)
    {
        _math.Mutate(_trajectories[i]);
    }
    
    return Task.FromResult(IterationResult.IterationFinished);
}
```

![Результат в dotMemory после хранения точек в массивах](./img/dotmemory-4.png)

Отлично, у сборщика мусора почти нет работы, нам не нужно лишний раз тратить вычислительные ресурсы.

Результаты бенчмаркинга:

```
|  Method | PointsCount | Epsilon |         Mean |      Error |     StdDev |
|-------- |------------ |-------- |-------------:|-----------:|-----------:|
| Execute |        5000 |    0.01 |    583.69 ms |  12.790 ms |   8.460 ms |
| Execute |        5000 |     0.1 |     56.79 ms |   6.693 ms |   4.427 ms |
| Execute |       50000 |    0.01 | 10,261.65 ms | 207.660 ms | 123.575 ms |
| Execute |       50000 |     0.1 |    921.83 ms |  48.425 ms |  32.030 ms |
```

С момента первой версии была увеличена точность нахождения решения, что сказалось на времени исполнения.
Для объективного сравнения решений в первую версию были внесены соответствующие изменения.

```csharp
public double NextSigned => (_random.NextDouble() * 2 - 1) * _accuracy * 10;
```

Сравним с результатами измененной первой версии:

```
|  Method | PointsCount | Epsilon |         Mean |      Error |     StdDev |
|-------- |------------ |-------- |-------------:|-----------:|-----------:|
| Execute |        5000 |    0.01 |  1,335.05 ms |  73.140 ms |  43.525 ms |
| Execute |        5000 |     0.1 |     52.83 ms |   9.424 ms |   5.608 ms |
| Execute |       50000 |    0.01 | 18,241.40 ms | 442.830 ms | 292.905 ms |
| Execute |       50000 |     0.1 |    768.00 ms |  52.173 ms |  34.510 ms |
```

## Выводы

На низкой точности новая версия стала работать незначительно дольше.
Это связано с большим относительным временем сетапа.
В новой версии мы сначала создаем все массивы, на что уходит время.
В старой версии сетап проще и на маленьких объемах это не несет последствий.

На высокой точности относительное время сетапа меньше.
В новой версии после сетапа не создаются новые объекты, поэтому программе не нужно останавливаться для сбора мусора.
Зато в старой версии это происходит постоянно, из-за чего время выполнения сильно увеличивается.
