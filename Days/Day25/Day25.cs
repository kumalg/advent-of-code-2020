using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day25 : Day<long> {
        private static int GetLoopSize(long subjectNumber, long publicKey) {
            long temporaryPublicKey = 1;
            int loopSize = 0;
            while (temporaryPublicKey != publicKey) {
                loopSize++;
                temporaryPublicKey *= subjectNumber;
                temporaryPublicKey %= 20201227;
            }
            return loopSize;
        }

        public override long FirstStar() {
            var inputs = InputLines.Select(int.Parse).ToArray();
            int cardLoopSize = GetLoopSize(7, inputs[0]);

            long temporaryEncryptionKey = 1;
            for (var i = 0; i < cardLoopSize; i++) {
                temporaryEncryptionKey *= inputs[1];
                temporaryEncryptionKey %= 20201227;
            }

            return temporaryEncryptionKey;
        }

        public override long SecondStar() {
            return 0;
        }
    }
}
