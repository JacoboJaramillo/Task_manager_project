export interface Task {
    id: string,
    title: string,
    description: string,
    isCompleted: boolean,
    priority: string,
    dueDate?: string | null,
    userId: string
}