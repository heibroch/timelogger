import { TableEntry } from "../api/TableEntry";

const BASE_URL = "https://localhost:7209";

//Neat that it has an async/await functionality, but not great that it still runs single threaded. Caused a bit of a problem for me.
export async function getAll(): Promise<TableEntry[]>
{
    const response = await fetch(`${BASE_URL}/projects/all`);
    return await response.json();
}