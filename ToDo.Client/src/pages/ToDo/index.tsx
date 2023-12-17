import { useState, useEffect, SetStateAction} from "react";
import {
    TextField,
    Button,
    Typography,
    Checkbox,
    List,
    ListItem,
    Container,
    makeStyles
} from "@mui/material";
//import { makeStyles } from 'tss-react/mui';
import "./styles.css";
import {TodoItem} from "../../models";
import {todoApiService} from "../../services";

const useStyles = makeStyles({
    input: {
        width: "70%",
        marginBottom: 30
    },
    addButton: {
        height: 55,
        marginBottom: 30
    },
    container: {
        textAlign: "center",
        marginTop: 100
    },
    list: {
        width: "80%",
        margin: "auto",
        display: "flex",
        justifyContent: "space-around",
        border: "1px solid light-gray"
    },
    text: {
        width: "70%"
    },
    listButtons: {
        marginLeft: 10
    }
});

export const ToDo : React.FC = () => {
    const [inputVal, setInputVal] = useState("");
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const classes = useStyles();


    useEffect(() => {
        const {getTodos} = todoApiService();
        getTodos().then((result) => {
            // if (result.status !== 'success') {
            //     history.push('/login');
            //     return;
            // }
            setTodos(result.data!);
        });
    }, [history]);

    const onChange = (e: { target: { value: SetStateAction<string>; }; }) => {
        setInputVal(e.target.value);
    };
    const handleCreate = async() => {

        const todo: TodoItem = {
            toDoId: '',
            title: inputVal,
            isDone: false,
        };

        const response = await todoApiService().createTodo(todo);

        setTodos([...todos, response.data!]);

        setInputVal("");
    };

    const onDelete = async (id: string) => {
        const response = await todoApiService().deleteTodo(id);
        
        if(response.status !== 'success') return;
        const newTodos = todos.filter((todo) => todo.toDoId !== id);
        setTodos(newTodos);
    };

    const handleDone = async (todo: TodoItem) => {
        const updated = todos.map((todo) => {
            if (todo.toDoId === todo.toDoId) {
                todo.isDone = !todo.isDone;
            }
            return todo;
        });
        await todoApiService().updateTodo(todo);
        setTodos(updated);
    };


    return (
        <Container component="main" className={classes.container}>
            <TextField
                variant="outlined"
                onChange={onChange}
                label="type your task to do"
                value={inputVal}
                className={classes.input}
            />
            <Button
                size="large"
                //variant={isEdited ? "outlined" : "contained"}
                color="primary"
                onClick={handleCreate}
                className={classes.addButton}
            >
                "Create Task"
            </Button>
            <List>
                {todos.map((todo : TodoItem) => {
                    return (
                        <>
                            <ListItem divider="bool" className={classes.list}>
                                <Checkbox
                                    onClick={() => handleDone(todo)}
                                    checked={todo.isDone}
                                />
                                <Typography
                                    className={classes.text}
                                    style={{ color: todo.isDone ? "green" : "" }}
                                    key={todo.toDoId}
                                >
                                    {todo.title}
                                </Typography>
                                <Button
                                    onClick={() => onDelete(todo.toDoId)}
                                    color="secondary"
                                    variant="contained"
                                    className={classes.listButtons}
                                >
                                    delete
                                </Button>
                            </ListItem>
                        </>
                    );
                })}
            </List>
        </Container>
    );
}

export default ToDo;
