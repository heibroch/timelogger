import { TableEntry } from '../api/TableEntry';
import { getAll } from '../api/projects';
import React from "react";

export default function Table() {
  
    let apiResponse = getAll(); //Don't know how I'd block the thread until render. Read about react components ability to trigger on mounting into the DOM, but there wasn't enough time for me to cover it
    let tableEntry1 = new TableEntry(1, 0, "MyProject", false, new Date(), new Date());
    let tableEntries: TableEntry[] = [tableEntry1];

    apiResponse.then(response => {
        tableEntries = response; //Didn't manage to get over that one.
    });

    console.log(tableEntries);

    return (
        <table>
            <thead className="bg-gray-200">
                <tr>
                    <th className="border px-4 py-2 w-12">#</th>
                    <th className="border px-4 py-2">Project Name</th>
                    <th className="border px-4 py-2">Finished</th>
                    <th className="border px-4 py-2">Started</th>
                    <th className="border px-4 py-2">Stopped</th>
                </tr>
            </thead>
            <tbody>
                {tableEntries.map(row => (
                    <tr>
                        <td className="border px-4 py-2 w-12">{row.id}</td>
                        <td className="border px-4 py-2">{row.projectName}</td>
                        <td className="border px-4 py-2">{row.projectCompleted.toString()}</td>
                        <td className="border px-4 py-2">{row.workStarted.toString()}</td>
                        <td className="border px-4 py-2">{row.workStopped.toString()}</td>
                    </tr>
                ))}     
            </tbody>
        </table>
    );
}
