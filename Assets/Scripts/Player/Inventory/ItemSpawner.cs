using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject food;
    [SerializeField] private GameObject medicine;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject bottle;
    [SerializeField] private GameObject jeff;

    [SerializeField] private Camera playerCamera;

    [Command("qq-spawn-entity-at-crosshair", "Spawns entity at the point you're looking at")]
    private void SpawnEntityAtCrosshair(string entityName, int amount)
    {
        SpawnOnNetworkServerRpc(entityName, amount);
    }

    // Tell server to spawn objects at position the palyer is looking at (crosshair).
    [ServerRpc(RequireOwnership = true)]
    private void SpawnOnNetworkServerRpc(string entityName, int amount)
    {
        GameObject entity;

        // tusen gånger bättre om man gör en dictionary men det kommer ändå vara lika manuellt arbete att lägga till grejer
        switch (entityName.ToLower())
        {
            case "water":
                entity = water;
                break;
            case "food":
                entity = food;
                break;
            case "medicine":
                entity = medicine;
                break;
            case "key":
                entity = key;
                break;
            case "bottle":
                entity = bottle;
                break;
            case "jeff":
                entity = jeff;
                amount = 1; // force 1 to spawn
                break;
            default:
                Debug.Log("Entity you're trying to spawn does not exist");
                return;
        }

        Physics.Raycast(playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit rayHit);

        var directionToSpawn = playerCamera.transform.position - rayHit.point;

        do
        {
            var localEntity = Instantiate(entity, rayHit.point, Quaternion.LookRotation(directionToSpawn, Vector3.up));

            Debug.Log("Enitity spawned at :" + localEntity.transform.position);

            localEntity.GetComponent<NetworkObject>().Spawn();

            amount--;

        } while (amount > 0);
    }

    // Could be done as said earlier with a dictionary but no need to make it too complicated
    [Command("qq-get-spawnable-entities", "Lists all spawnable entities")]
    private void ListSpawnableEntities()
    {
        Debug.Log("Spawnable entities: Water, Food, Medicine, Key, Bottle, Jeff");
    }
}