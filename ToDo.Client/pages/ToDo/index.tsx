import {
    TextField,
    Button,
    Typography,
    Checkbox,
    List,
    ListItem,
    Container,
    makeStyles
} from "@material-ui/core";
import { useState, useEffect } from "react";
import "./styles.css";
import { TodoItem } from "../../models";

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

function ToDo() {
    const [inputVal, setInputVal] = useState("");
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const classes = useStyles();


    useEffect(() => {
        const { getTodos } = todoApiFactory();
        getTodos().then((result) => {
            if (result.status !== 'success') {
                history.push('/login');
                return;
            }
            setTodos(result.data!);
        });
    }, [history]);
    
    const onChange = (e) => {
        setInputVal(e.target.value);
    };
    const handleCreate = async() => {
             
            const todo: TodoItem = {
                id: 0,
                title: inputVal,
                completed: false,
            };
            
            const response = await createTodoItem(todo);
            
            setTodos([...todos, response.data]);
       
        setInputVal("");
    };

    const onDelete = (id) => {
        const newTodos = todos.filter((todo) => todo.id !== id);
        setTodos(newTodos);
    };

    const handleDone = (id) => {
        const updated = todos.map((todo) => {
            if (todo.id === id) {
                todo.isDone = !todo.isDone;
            }
            return todo;
        });
        await updateTodo(id);
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
                                    onClick={() => handleDone(todo.toDoId)}
                                    checked={todo.isDone}
                                />
                                <Typography
                                    className={classes.text}
                                    style={{ color: todo.isDone ? "green" : "" }}
                                    key={todo.toDoId}
                                >
                                    {todo.val}
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
