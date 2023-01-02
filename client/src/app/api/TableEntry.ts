//Would likely have structured it further into classes
export class TableEntry{
    constructor(public id: Number, public projectId: Number, public projectName: string, public projectCompleted: boolean, public workStarted: Date, public workStopped: Date) {
        this.id = id;
        this.projectId = projectId;
        this.projectName = projectName;
        this.projectCompleted = projectCompleted;
        this.workStarted = workStarted;
        this.workStopped = workStopped;
    }
}