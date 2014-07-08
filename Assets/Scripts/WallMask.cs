//Use AND for remove and checking walls (protected and removed)
//Use OR for setting Drawn
public enum WallMask : byte {
    NorthRemoval = 191,     //10111111
    EastRemoval = 239,      //11101111
    SouthRemoval = 251,     //11111011
    WestRemoval = 254,      //11111110
    NorthDrawn = 128,     //10000000
    EastDrawn = 32,       //00100000
    SouthDrawn = 8,       //00001000
    WestDrawn = 2,        //00000010
    NorthRemovedCheck = 64, //01000000
    EastRemovedCheck = 16,  //00010000
    SouthRemovedCheck = 4,  //00000100
    WestRemovedCheck = 1,   //00000001
}