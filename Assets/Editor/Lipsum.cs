namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    public static class Lipsum
    {
        static int head = 0;
        static string[] lipsum = {  "lorem", "ipsum", "dolor", "sit", "amet", "consectetur",
                                    "adipiscing", "elit", "vivamus", "laoreet", "augue",
                                    "scelerisque", "nulla", "donec", "ante", "sollicitudin",
                                    "pulvinar", "nulla", "eget", "semper", "eros", "integer",
                                    "metus", "sapien", "congue", "vel", "massa", "gravida",
                                    "rhoncus", "sem", "justo", "purus", "fermentum",
                                    "sed", "justo", "non", "condimentum", "gravida", "urna",
                                    "praesent", "venenatis", "neque", "non", "dolor", "tincidunt",
                                    "fringilla", "aliquam", "arcu", "eros", "dictum", "vel",
                                    "facilisis", "consectetur", "sed", "nunc", "nam", "sit",
                                    "amet", "orci", "turpis"};

        public static string Next {
            get {
                int next = head;
                head = (head + 1) % lipsum.Length;
                return lipsum[next];
            }
        }
    }
}
