namespace MLPGPPParser;

public enum Label {
    Genre,
    Tags,
    Status,
    Engine,
    Character,
    Playtime,
}

public enum Tag {
    
    //Genre
    visual_novel,
    puzzle,
    action,
    rpg,
    platformer,
    jrpg,
    Shoot_em_Up,
    arcade,
    pointANDclick,
    tamagotchi,
    adventure,
    shooter,
    clicker,
    karaoke,
    horror,
    detective,
    racing,
    open_world,
    side_scroller,
    survival,
    tower_defence,
    mmo,
    fighting,
    casual,
    sport,
    strategy,
    humor,
    quest,
    sandbox,
    simulation,
    dress_up,
    social,
    
    //Tags
    _2d,
    _3d,
    top_down,
    first_person,
    thrid_person,
    cash_grab,
    official,
    anthro,
    coop,
    online,
    
    //Status
    Released,
    In_Development,
    
    //Engine
    customORnone,
    flash,
    unity,
    ue,
    godot,
    gamemaker,
    construct,
    rpgmaker,
    RenPy,
    blender,
    roblox,
    html5,
    Java,
    Adventure_Game_Studio,
    FM2K,
    Z_Engine,
    
    //Character
    M6,
    CMC,
    Secondary_cast,
    Background,
    OC,
    
    //Playtime
    LESS_5_min,
    FROM_5m_TO_30m,
    FROM_30m_TO_2h,
    FROM_2h_TO_4h,
    FROM_4h_TO_10h,
    MORE_10h,
}

public class TagsCollection {

    public Dictionary<Label, Tag> Tags;
    
    public TagsCollection(IEnumerable<Tag> tags) {
        var tagsCollection = new Dictionary<Label, Tag>();
        
        foreach (var tag in tags) {
            tagsCollection.Add(FindLabel(tag), tag);
        }
        
        Tags = tagsCollection;
    }
    
    public Label FindLabel(Tag tag) {
        switch (tag) {
            case Tag.visual_novel: return Label.Genre;
            case Tag.puzzle: return Label.Genre;
            case Tag.action: return Label.Genre;
            case Tag.rpg: return Label.Genre;
            case Tag.platformer: return Label.Genre;
            case Tag.jrpg: return Label.Genre;
            case Tag.Shoot_em_Up: return Label.Genre;
            case Tag.arcade: return Label.Genre;
            case Tag.pointANDclick: return Label.Genre;
            case Tag.tamagotchi: return Label.Genre;
            case Tag.adventure: return Label.Genre;
            case Tag.shooter: return Label.Genre;
            case Tag.clicker: return Label.Genre;
            case Tag.karaoke: return Label.Genre;
            case Tag.horror: return Label.Genre;
            case Tag.detective: return Label.Genre;
            case Tag.racing: return Label.Genre;
            case Tag.open_world: return Label.Genre;
            case Tag.side_scroller: return Label.Genre;
            case Tag.survival: return Label.Genre;
            case Tag.tower_defence: return Label.Genre;
            case Tag.mmo: return Label.Genre;
            case Tag.fighting: return Label.Genre;
            case Tag.casual: return Label.Genre;
            case Tag.sport: return Label.Genre;
            case Tag.strategy: return Label.Genre;
            case Tag.humor: return Label.Genre;
            case Tag.quest: return Label.Genre;
            case Tag.sandbox: return Label.Genre;
            case Tag.simulation: return Label.Genre;
            case Tag.dress_up: return Label.Genre;
            case Tag.social: return Label.Genre;
            
            case Tag._2d: return Label.Tags;
            case Tag._3d: return Label.Tags;
            case Tag.top_down: return Label.Tags;
            case Tag.first_person: return Label.Tags;
            case Tag.thrid_person: return Label.Tags;
            case Tag.cash_grab: return Label.Tags;
            case Tag.official: return Label.Tags;
            case Tag.anthro: return Label.Tags;
            case Tag.coop: return Label.Tags;
            case Tag.online: return Label.Tags;
            
            case Tag.Released: return Label.Status;
            case Tag.In_Development: return Label.Status;
            
            case Tag.customORnone: return Label.Engine;
            case Tag.flash: return Label.Engine;
            case Tag.unity: return Label.Engine;
            case Tag.ue: return Label.Engine;
            case Tag.godot: return Label.Engine;
            case Tag.gamemaker: return Label.Engine;
            case Tag.construct: return Label.Engine;
            case Tag.rpgmaker: return Label.Engine;
            case Tag.RenPy: return Label.Engine;
            case Tag.blender: return Label.Engine;
            case Tag.roblox: return Label.Engine;
            case Tag.html5: return Label.Engine;
            case Tag.Java: return Label.Engine;
            case Tag.Adventure_Game_Studio: return Label.Engine;
            case Tag.FM2K: return Label.Engine;
            case Tag.Z_Engine: return Label.Engine;
            
            case Tag.M6: return Label.Character;
            case Tag.CMC: return Label.Character;
            case Tag.Secondary_cast: return Label.Character;
            case Tag.Background: return Label.Character;
            case Tag.OC: return Label.Character;
            
            case Tag.LESS_5_min: return Label.Playtime;
            case Tag.FROM_5m_TO_30m: return Label.Playtime;
            case Tag.FROM_30m_TO_2h: return Label.Playtime;
            case Tag.FROM_2h_TO_4h: return Label.Playtime;
            case Tag.FROM_4h_TO_10h: return Label.Playtime;
            case Tag.MORE_10h: return Label.Playtime;
            
            default: throw new Exception($"can't find label for {tag}");
        }
    }
}

public record Screenshot(uint BucketId, uint Id);

public record GameBuild(uint Id, string Version, string ReleaseDate);

public record GameData(
    uint Id,
    string Name,
    string ShortDescription,
    TagsCollection Tags,
    string ReleaseDate,
    string Author,
    Screenshot HeroScreenshot,
    Screenshot[] OtherScreenshots,
    GameBuild[] Builds);


[Serializable]
public class SerializedTagData {
    public string label;
    public string type;
}

[Serializable]
public record GameSerializedData {
    public int id;
    public string title;
    public string author;
    public string thumbnail;
    public string shortDescription;
    public string fullDescription;
    public SerializedTagData[] tags;
    public string playtime;
    public string url;
    public string dateAdded;
}
