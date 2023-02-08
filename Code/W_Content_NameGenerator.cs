using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal static class W_Content_NameGenerator
    {
        internal static void add_name_generators()
        {
            add_eh_names();
            add_yao_names();
            add_city_names();
            add_kingdom_names();
            add_culture_names();
            add_cultibook_names();
            add_S_B_names();
            add_kingdom_mottos();
        }

        private static void add_kingdom_mottos()
        {
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "kingdom_mottos",
                words_id = "kingdom_mottos",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_kingdom_motto_template";
            name_template.format = "{0}";
            name_template.children.Add(main_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("kingdom_mottos");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_S_B_names()
        {
            CW_Template_Elm prefix_name = new CW_Template_Elm()
            {
                id = "S_B_prefix",
                words_id = "thousand_common_words",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "S_B_main",
                words_id = "S_B_random_main_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm postfix_name = new CW_Template_Elm()
            {
                id = "S_B_postfix",
                words_id = "S_B_postfix_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_S_B_name_template";
            name_template.format = "{0}{1}{2}";
            name_template.children.Add(prefix_name);
            name_template.children.Add(main_name);
            name_template.children.Add(postfix_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("special_body_name");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_cultibook_names()
        {
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "cultibook_main",
                words_id = "cultibook_random_main_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm postfix_name = new CW_Template_Elm()
            {
                id = "cultibook_postfix",
                words_id = "cultibook_postfix_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_cultibook_name_template";
            name_template.format = "{0}{1}";
            name_template.children.Add(main_name);
            name_template.children.Add(postfix_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("cultibook_name");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_culture_names()
        {
            CW_Template_Elm prefix_name = new CW_Template_Elm()
            {
                id = "eh_culture_prefix",
                words_id = "thousand_common_words",
                free_val = 0.3f
            };
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "eh_culture_main",
                words_id = "hundreds_of_family_names",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm postfix_name = new CW_Template_Elm()
            {
                id = "eh_culture_postfix",
                words_id = "eh_culture_postfix_name",
                free_val = 0.8f
            };
            CW_Template_Elm fixed_name = new CW_Template_Elm()
            {
                id = "eh_culture_fixed",
                words_id = "eh_city_fixed_name",
                free_val = 0f
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_culture_name_template";
            name_template.format = "{[1+]0[0,1]}{1[2,0]}{[1+]2[0,0][1,1]}{[1+]3[1,0]}{4[2,1]}";
            name_template.children.Add(prefix_name);
            name_template.children.Add(main_name);
            name_template.children.Add(postfix_name);
            name_template.children.Add(postfix_name);
            name_template.children.Add(fixed_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("eastern_human_culture");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_kingdom_names()
        {
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "eh_kingdom_main",
                words_id = "eh_kingdom_main_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm postfix_name = new CW_Template_Elm()
            {
                id = "eh_kingdom_postfix",
                words_id = "eh_kingdom_postfix_name",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm fixed_name = new CW_Template_Elm()
            {
                id = "eh_kingdom_fixed",
                words_id = "eh_kingdom_fixed_name",
                free_val = 0
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_kingdom_name_template";
            name_template.format = "{0[0,0]}{[0+]1}{2[0,1]}";
            name_template.children.Add(main_name);
            name_template.children.Add(postfix_name);
            name_template.children.Add(fixed_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("eastern_human_kingdom");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_city_names()
        {
            CW_Template_Elm prefix_name = new CW_Template_Elm()
            {
                id = "eh_city_prefix",
                words_id = "thousand_common_words",
                free_val = 0.3f
            };
            CW_Template_Elm main_name = new CW_Template_Elm()
            {
                id = "eh_city_main",
                words_id = "hundreds_of_family_names",
                select_from_objects = Actions.CW_NameTemplateActions.must_select
            };
            CW_Template_Elm postfix_name = new CW_Template_Elm()
            {
                id = "eh_city_postfix",
                words_id = "eh_city_postfix_name",
                free_val = 0.8f
            };
            CW_Template_Elm fixed_name = new CW_Template_Elm()
            {
                id = "eh_city_fixed",
                words_id = "eh_city_fixed_name",
                free_val = 0f
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_city_name_template";
            name_template.format = "{[1+]0[0,1]}{1[2,0]}{[1+]2[0,0][1,1]}{[1+]3[1,0]}{4[2,1]}";
            name_template.children.Add(prefix_name);
            name_template.children.Add(main_name);
            name_template.children.Add(postfix_name);
            name_template.children.Add(postfix_name);
            name_template.children.Add(fixed_name);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("eastern_human_city");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_eh_names()
        {
            CW_Template_Elm family_name = new CW_Template_Elm()
            {
                id = "human_fn",
                words_id = "hundreds_of_family_names",
                get_part_from_objects = Actions.CW_NameTemplateActions.descend_family_name
            };
            CW_Template_Elm middle_name = new CW_Template_Elm()
            {
                id = "human_mn",
                words_id = "thousand_common_words"
            };
            CW_Template_Elm last_name = new CW_Template_Elm()
            {
                id = "human_ln",
                words_id = "thousand_common_words"
            };
            CW_Template_Elm double_words_name_part = new CW_Template_Elm()
            {
                id = "human_dwn",
                words_id = "human_double_words_last_name",
                free_val = 0f
            };
            CW_Template name_template = new CW_Template();
            name_template.weight = 1;
            name_template.id = "default_human_name_template";
            name_template.format = "{0}{1[0,0]}{[1+]2}{3[0,1]}";
            name_template.children.Add(family_name);
            name_template.children.Add(middle_name);
            name_template.children.Add(last_name);
            name_template.children.Add(double_words_name_part);
            CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator("eastern_human_name");
            name_generator.add_template(name_template);
            CW_Library_Manager.instance.name_generators.add(name_generator);
        }

        private static void add_yao_names()
        {
            foreach(string yao_id in W_Content_Helper.yaos)
            {
                string name_template_id = AssetManager.unitStats.get(yao_id + "_yao").nameTemplate;
                CW_Template_Elm yao_family_name = new CW_Template_Elm()
                {
                    id = "family_name",
                    words_id = yao_id + "_yao_postfix",
                    select_from_objects = Actions.CW_NameTemplateActions.must_select,
                    get_part_from_objects = Actions.CW_NameTemplateActions.descend_family_name
                };
                CW_Template_Elm yao_main_name = new CW_Template_Elm()
                {
                    id = "main_name",
                    words_id = "yao_main_name",
                    select_from_objects = Actions.CW_NameTemplateActions.must_select
                };
                CW_Template name_template = new CW_Template();
                name_template.weight = 1;
                name_template.id = "default";
                name_template.format = "{0}{1}";
                name_template.children.Add(yao_main_name);
                name_template.children.Add(yao_family_name);
                CW_Asset_NameGenerator name_generator = new CW_Asset_NameGenerator(name_template_id);
                name_generator.add_template(name_template);
                CW_Library_Manager.instance.name_generators.add(name_generator);
            }
        }
    }
}
