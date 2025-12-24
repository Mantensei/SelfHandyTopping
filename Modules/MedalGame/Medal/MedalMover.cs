using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalMover : MonoBehaviour, IMoverProvider, IMoverEntity
    {
        [GetComponent(HierarchyRelation.Self | HierarchyRelation.Parent)]
        PlayerReferenceHub _player;
        public Rigidbody2D rb2d => _player.rb2d;
        public IMoverEntity Reference => this;

        public void Move(MoveCommand command)
        {
            rb2d.linearVelocity = command.Direction;
        }
    }
}
