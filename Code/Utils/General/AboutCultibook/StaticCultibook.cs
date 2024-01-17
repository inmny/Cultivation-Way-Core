using System.Collections.Generic;
using Cultivation_Way.Library;

namespace Cultivation_Way.Utils.General.AboutCultibook;

public static class StaticCultibook
{
    public static Cultibook create_and_add_cultibook(
        string name, string description,
        string author_name, string editor_name,
        int level,
        int max_users = Constants.Others.cultibook_lock_line + 1,
        List<string> spells = null,
        int more_spell_allowed = 0
    )
    {
        Cultibook cultibook = new()
        {
            name = name,
            description = description,
            author_name = author_name,
            editor_name = editor_name,
            level = level,
            spells = spells ?? new List<string>(),
            max_spell_nr = (spells?.Count ?? 0) + more_spell_allowed,
            max_users = max_users
        };
        Manager.cultibooks.add_to_static_list(cultibook);
        return cultibook;
    }
}