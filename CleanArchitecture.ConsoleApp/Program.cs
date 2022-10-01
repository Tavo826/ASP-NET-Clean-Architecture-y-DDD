using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();

//await insertDataBasic();
//QueryStreaming();
//await QueryMethods();
//await QueryFilter();
//await QueryLinq();
//await TrakingAndNotTraking();

// await AddNewStreamerWithVideo();
// await AddNewActorWithVideo();
// await AddNewDirectorWithVideo();
await MultipleEntitiesQuery();


Console.WriteLine("Presione cualquier tecla...");
Console.ReadKey();


async Task MultipleEntitiesQuery()
{
    //var videoWithActores = await dbContext!.Videos!.Include(q => q.Actores).FirstOrDefaultAsync(q => q.Id == 1);

    // devuelve solamente la columna nombre
    //var actor = await dbContext!.Actores!.Select(q => q.Nombre).ToListAsync();

    var videoWithDirector = await dbContext!.Videos!
        .Where(q => q.Director != null)
        .Include(q => q.Director)
        .Select(q =>
            new
            {
                DirectorNombreCompleto = $"{q.Director.Nombre} {q.Director.Apellido}",
                VideoNombre = q.Nombre
            }
        ).ToListAsync();

    foreach (var video in videoWithDirector)
    {
        Console.WriteLine($"{video.VideoNombre} - {video.DirectorNombreCompleto}");
    }
}

async Task AddNewDirectorWithVideo()
{
    var director = new Director
    {
        Nombre = "Lorenzo",
        Apellido = "Basteri",
        VideoId = 1
    };

    await dbContext.AddAsync(director);
    await dbContext.SaveChangesAsync();
}

async Task AddNewActorWithVideo()
{
    var actor = new Actor
    {
        Nombre = "Brad",
        Apellido = "Pitt"
    };

    await dbContext.AddAsync(actor);
    await dbContext.SaveChangesAsync();

    var videoActor = new VideoActor
    {
        ActorId = actor.Id,
        VideoId = 1
    };

    await dbContext.AddAsync(videoActor);
    await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithVideo()
{
    var pantalla = new Streamer
    {
        Nombre = "Pantaya"
    };

    var hungerGames = new Video
    {
        Nombre = "Hunger Games",
        Streamer = pantalla
    };

    await dbContext.AddAsync(hungerGames);
    await dbContext.SaveChangesAsync();
}

async Task TrakingAndNotTraking()
{
    var streamerWithTraking = await dbContext.Streamers.FirstOrDefaultAsync(streamer => streamer.Id == 1);
    var streamerWithNoTraking = await dbContext.Streamers.AsNoTracking().FirstOrDefaultAsync(streamer => streamer.Id == 1);

    // Permite actualizar el nombre
    streamerWithTraking.Nombre = "Netflix Super";
    // No permite actualizar el nombre, el objeto se ha liberado de la memoria temporal
    streamerWithNoTraking.Nombre = "Amazon Plus";

    await dbContext.SaveChangesAsync();
}
    
async Task QueryLinq()
{
    Console.Write($"Ingrese el servicio de streaming");
    var streamerNombre = Console.ReadLine();

    var streamers = await (from i in dbContext.Streamers
                           where EF.Functions.Like(i.Nombre, $"%{streamerNombre}%")
                           select i).ToListAsync();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task QueryFilter()
{
    Console.WriteLine($"Ingrese una compañia de streaming: ");
    var streamingNombre = Console.ReadLine();

    // Buscando un streamer
    // ToListAsync -> colección de datos a lista
    var result = await dbContext.Streamers.Where(streamer => streamer.Nombre.Equals(streamingNombre)).ToListAsync();
    foreach (var streamer in result)
    {
        Console.WriteLine($"Resultado: {streamer.Id} - {streamer.Nombre}");
    }

    // var streamerPartialResult = await dbContext.Streamers.Where(streamer => streamer.Nombre.Contains(streamingNombre)).ToListAsync();
    // $"%{streamingNombre}%" -> busca los elementos que coincida ese nombre
    var streamerPartialResult = await dbContext.Streamers.Where(streamer => EF.Functions.Like(streamer.Nombre, $"%{streamingNombre}%")).ToListAsync();

    foreach (var streamer in streamerPartialResult)
    {
        Console.WriteLine($"Resultado: {streamer.Id} - {streamer.Nombre}");
    }
}

async Task QueryMethods()
{
    var streamer = dbContext.Streamers;

    // Asume que existe la data y si no lo encuentra dispara excepción
    var firstAsync = await streamer.Where(streamer => streamer.Nombre.Contains("a")).FirstAsync();
    // No asume que existe la data y si no lo encuentra devuelve un valor por defecto en null
    var firstOrDefaultAsync = await streamer.Where(streamer => streamer.Nombre.Contains("a")).FirstOrDefaultAsync();
    var firstOrDefaultAsync2 = await streamer.FirstOrDefaultAsync(streamer => streamer.Nombre.Contains("a"));

    // El resultado debe ser un solo valor, si es una colección da excepción
    var singleAsync = await streamer.Where(streamer => streamer.Id == 1).SingleAsync();
    var singleOrDefaultAsync = await streamer.Where(streamer => streamer.Id == 1).SingleOrDefaultAsync();

    // busca por primaryKey
    var resultado = await streamer.FindAsync(1);
}

void QueryStreaming()
{
    var streamers = dbContext.Streamers.ToList();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task insertDataBasic()
{
    Streamer streamer = new()
    {
        Nombre = "Amazon Prime",
        Url = "https://www.amazonprime.com"
    };

    // Se agrega a la base de datos
    // ! -> el objeto existe y está instanciado
    dbContext!.Streamers!.Add(streamer);
    await dbContext.SaveChangesAsync();

    var movies = new List<Video>
    {
        new Video
        {
            Nombre = "Mad Max",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "Batman",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "Crepusculo",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "Citizen Kane",
            StreamerId = streamer.Id
        }
    };

    // agregando las películas
    await dbContext.AddRangeAsync(movies);
    await dbContext.SaveChangesAsync();
}


