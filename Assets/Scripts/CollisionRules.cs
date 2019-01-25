using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRules : MonoBehaviour {
    
    private struct CollisionInfo {
        GameObject a;
        GameObject b;

        public bool Equals(CollisionInfo other) {
            return a == other.a && b == other.b || a == other.b && b == other.a;
        }
    }

    private List<CollisionInfo> handledCollisions;


    public void HandleCollision(GameObject o, Collider other) {
        CharacterInfo characterInfo = o.GetComponent<CharacterInfo>();
        if (characterInfo) {
            HandleCollision(characterInfo, other);
            return;
        }
    }

    private void HandleCollision(CharacterInfo info, Collider other) {
        if (info is EnemyInfo) {
            HandleCollision((EnemyInfo)info, other);
            return;
        }
        PlayerCharacterController controller = info.GetComponent<PlayerCharacterController>();
        EnemyInfo enemyInfo = other.GetComponent<EnemyInfo>();
        if (enemyInfo) {
            switch (enemyInfo.Type) {
                case EnemyType.Blue:
                    BlueBlobBehaviour blob = other.GetComponent<BlueBlobBehaviour>();
                    if (blob.CurrentState.Equals(BlueBlobBehaviour.State.Pouncing) ||
                        blob.CurrentState.Equals(BlueBlobBehaviour.State.Eating)) {
                        controller.Mount(other.gameObject);
                        controller.Die();
                    }
                    else {
                        controller.Stagger();
                    }
                    break;
                default:
                    controller.Stagger();
                    break;
            }
        }
    }

    private void HandleCollision(EnemyInfo info, Collider other) {
        BlobBehaviour blob = info.GetComponent<BlobBehaviour>();

        // If it hits a weapon. It has to get hit.
        if (other.GetComponent<Weapon>()) {
            blob.GetHit();
        }

        // Did we collide with another Blob?
        BlobBehaviour otherBlob = other.GetComponent<BlobBehaviour>();
        if (otherBlob) {
            EnemyInfo otherInfo = other.GetComponent<EnemyInfo>();
            switch (otherInfo.Type) {
                case EnemyType.Green:
                    if (info.Type.Equals(EnemyType.Green)) {
                        // Do something nice...
                    }
                    break;
                case EnemyType.Blue:
                    switch (((BlueBlobBehaviour)otherBlob).CurrentState) {
                        case BlueBlobBehaviour.State.Pouncing:
                            blob.GetHit();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        // Special Cases:
        if (blob is BlueBlobBehaviour) {
            HandleCollision((BlueBlobBehaviour)blob, other);
            return;
        }

        // If we did not return already:
        // every Blob has to bounce away from anything it hits.
        BounceAway(blob, other);
    }

    private void HandleCollision(BlueBlobBehaviour blob, Collider other) {
        CharacterInfo otherInfo = other.GetComponent<CharacterInfo>();
        if (otherInfo) {
            switch (blob.CurrentState) {
                case BlueBlobBehaviour.State.Default:
                    BounceAway(blob, other);
                    break;
                case BlueBlobBehaviour.State.Pouncing:
                    if (otherInfo.Faction.Equals(Faction.Player)) {
                        blob.StartEating();
                    }
                    break;
                default:
                    break;
            }
        }
        else {
            BounceAway(blob, other);
        }
    }

    private void BounceAway(BlobBehaviour blob, Collider other) {
        Vector3 otherPos = other.gameObject.transform.position;
        Vector3 direction = blob.transform.position - otherPos;
        blob.TurnInstant(direction);
    }
}
