void main()
{
    import std;

    L10: writef("%26s", ' '); writeln("ACEY DUCEY CARD GAME");
    L20: writef("%15s", ' '); writeln("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    L21: writeln;
    L22: writeln;
    L30: writeln("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER ");
    L40: writeln("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP");
    L50: writeln("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING");
    L60: writeln("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE");
    L70: writeln("A VALUE BETWEEN THE FIRST TWO.");
    L80: writeln("IF YOU DO NOT WANT TO BET, INPUT A 0");
    L100: int N=100;
    L110: int Q=100, M;
    L120: writeln("YOU NOW HAVE ",Q," DOLLARS.");
    L130: writeln;
    L140: goto L260;
    L210: Q=Q+M;
    L220: goto L120;
    L240: Q=Q-M;
    L250: goto L120;
    L260: writeln("HERE ARE YOUR NEXT TWO CARDS: ");
    L270: auto A=to!int(14*uniform01)+2;
    L280: if (A<2) goto L270;
    L290: if (A>14) goto L270;
    L300: auto B=to!int(14*uniform01)+2;
    L310: if (B<2) goto L300;
    L320: if (B>14) goto L300;
    L330: if (A>=B) goto L270;
    L350: if (A<11) goto L400;
    L360: if (A==11) goto L420;
    L370: if (A==12) goto L440;
    L380: if (A==13) goto L460;
    L390: if (A==14) goto L480;
    L400: writefln("%2d", A);
    L410: goto L500;
    L420: writeln("JACK");
    L430: goto L500;
    L440: writeln("QUEEN");
    L450: goto L500;
    L460: writeln("KING");
    L470: goto L500;
    L480: writeln("ACE");
    L500: if (B<11) goto L550;
    L510: if (B==11) goto L570;
    L520: if (B==12) goto L590;
    L530: if (B==13) goto L610;
    L540: if (B==14) goto L630;
    L550: writefln("%2d", B);
    L560: goto L650;
    L570: writeln("JACK");
    L580: goto L650;
    L590: writeln("QUEEN");
    L600: goto L650;
    L610: writeln("KING");
    L620: goto L650;
    L630: writeln("ACE");
    L640: writeln;
    L650: writeln;
    L660: write("WHAT IS YOUR BET? "); M = stdin.readln.strip.to!int;
    L670: if (M!=0) goto L680;
    L675: writeln("CHICKEN!!");
    L676: writeln;
    L677: goto L260;
    L680: if (M<=Q) goto L730;
    L690: writeln("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.");
    L700: writeln("YOU HAVE ONLY ",Q," DOLLARS TO BET.");
    L710: goto L650;
    L730: auto C=to!int(14*uniform01)+2;
    L740: if (C<2) goto L730;
    L750: if (C>14) goto L730;
    L760: if (C<11) goto L810;
    L770: if (C==11) goto L830;
    L780: if (C==12) goto L850;
    L790: if (C==13) goto L870;
    L800: if (C==14) goto L890;
    L810: writeln(C);
    L820: goto L910;
    L830: writeln("JACK");
    L840: goto L910;
    L850: writeln("QUEEN");
    L860: goto L910;
    L870: writeln("KING");
    L880: goto L910;
    L890: writeln( "ACE");
    L900: writeln;
    L910: if (C>A) goto L930;
    L920: goto L970;
    L930: if (C>=B) goto L970;
    L950: writeln("YOU WIN!!!");
    L960: goto L210;
    L970: writeln("SORRY, YOU LOSE");
    L980: if (M<Q) goto L240;
    L990: writeln;
    L1000: writeln;
    L1010: writeln("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.");
    L1015: writeln;writeln;
    L1020: write("TRY AGAIN (YES OR NO)? "); auto AS=stdin.readln;
    L1025: writeln;writeln;
    L1030: if (AS.strip.toUpper=="YES") goto L110;
    L1040: writeln("O.K., HOPE YOU HAD FUN!");
}
