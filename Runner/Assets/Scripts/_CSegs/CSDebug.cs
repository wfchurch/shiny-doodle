using UnityEngine;

/// <summary>
/// corp 3 system defense levels... (all use some sort of detection)
///     1 firewall
///     2 intrusion detection systems (ids ) (filter, packet analizer,gate)
///     3 intrusion countermeasues systems (ics, aka ickies) (counter)
///     
/// corp retaliations
///     1 trackers
///     2 tracers
///     
/// corp data defense...
///     1 authentication
///     2 masking
///     3 encryption
///     
/// corp payload defense...
///     1 sweeper
///     2 antivirus
///     3 blackhole, sinkhole
///     3 reflector
///     3 deflector
///     
/// runner 3 matching attacks... (all have some kind of signature)
///     1 port scanner, loris
///     2 spoof, decoy, evasion
///     3 cracker, loop
///     
/// runner attack results...
///     1 steal data
///         3 decrypt vs encryption
///         2 sniffer vs masking
///         1 spy vs authentication
///         
///     2 deliver payload
///         2 virus
///         1 trojan
///         2,3 worm
///         1 zombie
///         1 bot
///         1 root
///         1 backdoor
///         3 bomb
///         3 lazer
///         
/// runner retaliation defense
///     1 zapper
///     2 proxy
///     
///         
/// </summary>

public class CSDebug : CodeSim {

    public CSDebug()
    {
        model = "Foo";
        manufacturer = "Bar";
        cycles = 2;
        size = 4;
        type = CodeType.codeOp;
    }
}
