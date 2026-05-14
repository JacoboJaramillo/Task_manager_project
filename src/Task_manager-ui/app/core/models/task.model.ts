export interface Task {
    id: string,
    title: string,
    description: string,
    isCompleted: boolean,
    priority: string,
    dueDate: Date,
    userId: string
}