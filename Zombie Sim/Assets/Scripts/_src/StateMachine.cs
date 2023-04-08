// set of states
public enum ZombieState
{
    Idle,           // just chilling
    Roaming,        // roaming around aimlessly
    Investigating,  // detected something & moves to its origin
    Stalking,       // has detected a target and pursues it
    Attacking,      // in grapple range of target, tries to bite
    Feeding,        // munching without a care in the world
    Incapacitated,  // brain destroyed or immobilized
};

// set of inputs (connections between states)
public enum ZombieAction
{
    Nothing,            // stimuli are absent for a while
    TargetOutOfRange,   // target is detected outside of attack range
    TargetWithinRange,  // target is within attack range
    TargetKilled,       // something (only engaged target?) is killed within detection range
    SoundLure,          // sound is heard
    LightLure,          // bright light is seen during the nighttime
    Incapacitate,       // 'health' is fully depleted
}

public class ZombieFSM
{
    public ZombieState state;

    ZombieState ChangeState(ZombieState state, ZombieAction action) => (state, action) switch
    {
        // idle stuff
        (ZombieState.Roaming, ZombieAction.Nothing) => ZombieState.Idle,     // walking sucks
        (ZombieState.Idle, ZombieAction.Nothing) => ZombieState.Roaming,     // standing sucks
        (ZombieState.Feeding, ZombieAction.Nothing) => ZombieState.Roaming,  // meat not fresh enough

        // investigating & stalking
        (ZombieState.Idle, ZombieAction.SoundLure) => ZombieState.Investigating,     // what tf was that?
        (ZombieState.Roaming, ZombieAction.SoundLure) => ZombieState.Investigating,
        (ZombieState.Idle, ZombieAction.LightLure) => ZombieState.Investigating,
        (ZombieState.Roaming, ZombieAction.LightLure) => ZombieState.Investigating,

        (ZombieState.Idle, ZombieAction.TargetOutOfRange) => ZombieState.Stalking,   // c'mere! I'm hungry!
        (ZombieState.Roaming, ZombieAction.TargetOutOfRange) => ZombieState.Stalking,
        (ZombieState.Investigating, ZombieAction.TargetOutOfRange) => ZombieState.Stalking,
        (ZombieState.Feeding, ZombieAction.TargetOutOfRange) => ZombieState.Stalking,

        (ZombieState.Investigating, ZombieAction.Nothing) => ZombieState.Roaming,    // losing interest
        (ZombieState.Stalking, ZombieAction.Nothing) => ZombieState.Roaming,         // losing interest?

        // attacking
        (ZombieState.Idle, ZombieAction.TargetWithinRange) => ZombieState.Attacking,
        (ZombieState.Roaming, ZombieAction.TargetWithinRange) => ZombieState.Attacking,
        (ZombieState.Investigating, ZombieAction.TargetWithinRange) => ZombieState.Attacking,
        (ZombieState.Feeding, ZombieAction.TargetWithinRange) => ZombieState.Attacking,
        (ZombieState.Stalking, ZombieAction.TargetWithinRange) => ZombieState.Attacking,

        (ZombieState.Attacking, ZombieAction.TargetKilled) => ZombieState.Feeding,       // meal prep
        (ZombieState.Attacking, ZombieAction.TargetOutOfRange) => ZombieState.Stalking,  // try to get in range again
        (ZombieState.Investigating, ZombieAction.TargetKilled) => ZombieState.Feeding,   // nevermind bc food
        (ZombieState.Stalking, ZombieAction.TargetKilled) => ZombieState.Feeding,        // nevermind bc food

        // being incapacitated
        (ZombieState.Idle, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        (ZombieState.Roaming, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        (ZombieState.Investigating, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        (ZombieState.Stalking, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        (ZombieState.Attacking, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        (ZombieState.Feeding, ZombieAction.Incapacitate) => ZombieState.Incapacitated,
        _ => throw new System.NotImplementedException(),
    };
}