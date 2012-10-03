using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public static class ModelFactory
    {
        public static List<string> GetWordList()
        {
            var words = new List<string>();
            words.Add("through");
            words.Add("another");
            words.Add("because");
            words.Add("picture");
            words.Add("America");
            words.Add("between");
            words.Add("country");
            words.Add("thought");
            words.Add("example");
            words.Add("without");
            words.Add("banging");
            words.Add("bathtub");
            words.Add("blasted");
            words.Add("blended");
            words.Add("bobsled");
            words.Add("camping");
            words.Add("contest");
            words.Add("dentist");
            words.Add("disrupt");
            words.Add("himself");
            words.Add("jumping");
            words.Add("lending");
            words.Add("pinball");
            words.Add("planted");
            words.Add("plastic");
            words.Add("problem");
            words.Add("ringing");
            words.Add("shifted");
            words.Add("sinking");
            words.Add("sunfish");
            words.Add("trusted");
            words.Add("twisted");
            words.Add("nothing");
            words.Add("chuckle");
            words.Add("fishing");
            words.Add("forever");
            words.Add("grandpa");
            words.Add("grinned");
            words.Add("grumble");
            words.Add("library");
            words.Add("perfect");
            words.Add("Sanchez");
            words.Add("snuggle");
            words.Add("sparkle");
            words.Add("sunburn");
            words.Add("swimmer");
            words.Add("tadpole");
            words.Add("twinkle");
            words.Add("whisper");
            words.Add("whistle");
            words.Add("writing");
            words.Add("applaud");
            words.Add("awesome");
            words.Add("bedtime");
            words.Add("beehive");
            words.Add("begging");
            words.Add("broiler");
            words.Add("careful");
            words.Add("collect");
            words.Add("crinkle");
            words.Add("cupcake");
            words.Add("delight");
            words.Add("explore");
            words.Add("giraffe");
            words.Add("gumball");
            words.Add("harmful");
            words.Add("helpful");
            words.Add("herself");
            words.Add("highway");
            words.Add("hopeful");
            words.Add("instead");
            words.Add("kneecap");
            words.Add("leather");
            words.Add("necktie");
            words.Add("nowhere");
            words.Add("oatmeal");
            words.Add("outside");
            words.Add("painful");
            words.Add("pancake");
            words.Add("pennies");
            words.Add("popcorn");
            words.Add("pretzel");
            words.Add("rainbow");
            words.Add("recycle");
            words.Add("restful");
            words.Add("sandbox");
            words.Add("scratch");
            words.Add("shaking");
            words.Add("silence");
            words.Add("smiling");
            words.Add("snowman");
            words.Add("someone");
            words.Add("splotch");
            words.Add("startle");
            words.Add("stiffer");
            words.Add("strange");
            words.Add("stretch");
            words.Add("sunrise");
            words.Add("teacher");
            words.Add("thimble");
            words.Add("thinner");
            words.Add("topcoat");
            words.Add("traffic");
            words.Add("treetop");
            words.Add("trouble");
            words.Add("trumpet");
            words.Add("Tuesday");
            words.Add("tugboat");
            words.Add("visitor");
            words.Add("weather");
            words.Add("weekend");
            words.Add("wettest");
            words.Add("wriggle");
            words.Add("wrinkle");
            words.Add("written");
            words.Add("brother");
            words.Add("present");
            words.Add("program");
            words.Add("twitter");
            words.Add("against");
            words.Add("general");
            words.Add("however");
            words.Add("airport");
            words.Add("anybody");
            words.Add("balloon");
            words.Add("bedroom");
            words.Add("bicycle");
            words.Add("brownie");
            words.Add("cartoon");
            words.Add("ceiling");
            words.Add("channel");
            words.Add("chicken");
            words.Add("garbage");
            words.Add("promise");
            words.Add("squeeze");
            words.Add("address");
            words.Add("blanket");
            words.Add("earache");
            words.Add("excited");
            words.Add("grandma");
            words.Add("grocery");
            words.Add("indoors");
            words.Add("January");
            words.Add("kitchen");
            words.Add("lullaby");
            words.Add("monster");
            words.Add("morning");
            words.Add("naughty");
            words.Add("October");
            words.Add("pajamas");
            words.Add("pretend");
            words.Add("quarter");
            words.Add("shampoo");
            words.Add("stomach");
            words.Add("tonight");
            words.Add("unhappy");
            words.Add("pumpkin");
            words.Add("printed");
            words.Add("planned");
            words.Add("spilled");
            words.Add("smelled");
            words.Add("grilled");
            words.Add("slammed");
            words.Add("spelled");

            return words;
        }

        public static List<Post> GetRandomPosts(int count)
        {
            List<Post> posts = new List<Post>();

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<string> words = GetWordList();

            for (int i = 0; i < count; i++)
            {
                DateTime d = DateTime.Now.AddDays(rnd.Next(0, 60) - 30);
                string s = words[rnd.Next(0, words.Count - 1)];
                double v = rnd.NextDouble()*100;
                posts.Add(new Post(d, s, v));
            }

            return posts;
        }
    }
}
