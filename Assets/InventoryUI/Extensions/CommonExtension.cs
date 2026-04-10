using System.Collections.Generic;
using System.Linq;

public static class CommonExtension {

    private static readonly System.Random _random = new ();
    
    public static IEnumerable<T> Shuffle<T> (this IEnumerable<T> array, int count = 1) {
        var buffer = array.ToArray();
        
        
        for(var i =0; i < count; i++) {
            var n = buffer.Length;
            while (n > 1) {
                var k = _random.Next(n--);
                (buffer[n], buffer[k]) = (buffer[k], buffer[n]);
            }
        }

        return buffer;
    }
    
}
