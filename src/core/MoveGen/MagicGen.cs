
namespace Chess.Core{
    public static class MagicGen{

        public static int[] rBits = {
        12, 11, 11, 11, 11, 11, 11, 12,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        12, 11, 11, 11, 11, 11, 11, 12
        };

        public static int[] bBits = {
        6, 5, 5, 5, 5, 5, 5, 6,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 7, 7, 7, 7, 5, 5,
        5, 5, 7, 9, 9, 7, 5, 5,
        5, 5, 7, 9, 9, 7, 5, 5,
        5, 5, 7, 7, 7, 7, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        6, 5, 5, 5, 5, 5, 5, 6
        };

        public static ulong[] rMagics = {756609410333151360, 4629709350474088768, 72092779500540424, 360296930296733952, 5260221978433947648, 144119895637164544, 72067489710047744, 144115331991078977, 581105091570827296, 4612952930702590080, 1748241217789954113, 5822591789144489992, 20406970172571776, 9223653546795205376, 81346285466067456, 2522438076807004800, 5489311805866528, 580971223914119234, 288319437702955776, 1729454825248174088, 362822344760367104, 1153204079162296322, 1163059001949229062, 299504768659805956, 1369164657612325408, 2310349083817058304, 292751847144292608, 2309264691547865216, 1317865866729627793, 4622949417689940096, 8881992515856, 1444548297946596420, 141012424984608, 4612957878511214592, 585749701765111808, 1157469086855204864, 2278190248625152, 2882867089461227524, 252212574265868292, 36031288133558401, 1621296278175531012, 4612002746500136960, 1158062280114569250, 351289639002308640, 289637785395331120, 2814784194609232, 844845836992516, 10412395183609610241, 1693387782686208, 36592034922398208, 668511932320000, 445997134958821504, 10366197774680192, 9570767755280896, 4683750382676034560, 695811641069273600, 88102706085955, 1171253666544452098, 307098065513218305, 9804345940799193285, 2450521766859661330, 4035788250458394658, 2233651603716, 144233936430604546};
        public static ulong[] bMagics = {725396207953906001, 22519651766403073, 47573673506242560, 4615083827185058880, 299342309393408, 2310651208995442688, 294714751603200, 40602937256452096, 1152956698743407616, 290517643874601510, 1134842097311745, 10141914599264280, 9296029973112422914, 149842853635088, 315256408504275200, 72137860131723264, 1227233238176498176, 217300915672318016, 4760340686354518049, 9364109559604453888, 9223517739335812672, 288793533342687249, 2635731759261631488, 281668812817408, 598752952324097, 577623211609952516, 4612046659521085696, 74318198536675360, 142941947495424, 5634997631361024, 2425368719934095892, 4972608140555126796, 4837885833388160, 163256878353105936, 3197627244525650944, 1163285450195072, 71469330333760, 306403108635939076, 9225632637324906773, 92609670350831874, 11538787737972785409, 9583942109625331722, 9152352053363200, 1152939380336232448, 1154139780806607872, 378338354099388544, 4648754556698752, 3459895378726879300, 285877859778564, 9232661132261744704, 4557545508175872, 5638468534666241, 9229003254410119170, 576465189273403776, 13839597148527696144, 1155182257883856976, 295058350072668672, 282170796057664, 9228438749665454081, 2305843009754890761, 2343598049160866305, 153195367951834688, 28302597808391424, 6773610672824354};
        public static ulong genMagicNumber(Bitboard[] blockers, Square square, bool isRook){
            
            ulong magicNum = 0;
            int shift = isRook ? rBits[(int)square] : bBits[(int)square];
            bool magicNumFound = false;
            Random r = new();

            if(square == Square.e3)
                Console.WriteLine("Doing e3");

            while(!magicNumFound){
                magicNumFound = true;
                Bitboard[] check = new Bitboard[blockers.Length];
                

                ulong a = (ulong)r.NextInt64() | ((ulong)r.NextInt64() << 63);
                ulong b = (ulong)r.NextInt64() | ((ulong)r.NextInt64() << 63);
                ulong c = (ulong)r.NextInt64() | ((ulong)r.NextInt64() << 63);

                magicNum = a & b & c;

                for(int i = 0; i < blockers.Length; i++){
                    
                    int index = (int)(blockers[i].bitboard * magicNum >> 64-shift);
                    Bitboard moveMask = isRook ?  
                                        BlockerBoard.genRookMoveMask(blockers[i], square) :
                                        BlockerBoard.genBishopMoveMask(blockers[i], square); 

                    if(check[index].bitboard != 0 && check[index].bitboard != moveMask.bitboard){
                        magicNumFound = false;
                        break;
                    }
                    check[index] = moveMask;
                }
            }

            return magicNum;
        }

        public static ulong[] genRookMagics(){

            ulong[] magics = new ulong[64];
            Bitboard[] rook = AttackGenerator.generateRookAttackMasks();

            for(int i = 0; i < magics.Length; i++){
                magics[i] = genMagicNumber(BlockerBoard.blockerMasks(rook[i], (Square)i, true), (Square)i, true);
            }

            return magics;
        }

        public static ulong[] genBishopMagics(){

            ulong[] magics = new ulong[64];
            Bitboard[] bishop = AttackGenerator.generateBishopAttackMasks();

            for(int i = 0; i < magics.Length; i++){
                magics[i] = genMagicNumber(BlockerBoard.blockerMasks(bishop[i], (Square)i, false), (Square)i, false);
            }

            return magics;
        }

        public static Bitboard[][] makeRookDict(){
           
            Bitboard[][] dict = new Bitboard[64][];
            Bitboard[] rook = AttackGenerator.generateRookAttackMasks();

            for(int i = 0; i < rMagics.Length; i++){

                

                Bitboard[] blockers = BlockerBoard.blockerMasks(rook[i], (Square)i, true);
                Bitboard[] rookMoveMasks = new Bitboard[blockers.Length];
                
                for(int j = 0; j < blockers.Length; j++){
                    rookMoveMasks[blockers[j].bitboard * rMagics[i] >> (64-rBits[i])] = BlockerBoard.genRookMoveMask(blockers[j].bitboard, (Square)i);
                }
                dict[i] = rookMoveMasks;
            }

            return dict;
        }

        public static Bitboard[][] makeBishopDict(){

            Bitboard[][] dict = new Bitboard[64][];
            Bitboard[] bishop = AttackGenerator.generateBishopAttackMasks();

            for(int i = 0; i < rMagics.Length; i++){

                Bitboard[] blockers = BlockerBoard.blockerMasks(bishop[i], (Square)i, false);
                Bitboard[] bishopMoveMasks = new Bitboard[blockers.Length];
                
                for(int j = 0; j < blockers.Length; j++){
                    bishopMoveMasks[blockers[j].bitboard * bMagics[i] >> (64-bBits[i])] = BlockerBoard.genBishopMoveMask(blockers[j], (Square)i);
                }
                dict[i] = bishopMoveMasks;
            }

            return dict;
        }
    }
}