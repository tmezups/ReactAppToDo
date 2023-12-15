import { useEffect, useState } from 'react';
import './App.css';

interface ToDoItem {
    id: string;
    title: string;
    isDone: boolean;
    createdOn: Date;
    updatedOn: Date;
}

function App() {
    const [todoItems, setToDoItems] = useState<ToDoItem[]>();

    useEffect(() => {
        populateToDoData();
    }, []);

    const contents = todoItems === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th>IsDone</th>
                    <th>Created On</th>
                    <th>Updated On</th>
                </tr>
            </thead>
            <tbody>
                {todoItems.map(todo =>
                    <tr key={todo.id}>
                        <td>{todo.id}</td>
                        <td>{todo.title}</td>
                        <td>{todo.isDone.toString()}</td>
                        <td>{todo.createdOn.toString()}</td>
                        <td>{todo.updatedOn.toString()}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">ToDo</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function populateToDoData() {
        const response = await fetch('ToDo');
        const data = await response.json();
        console.log(data);
        setToDoItems(data);
    }
}

export default App;