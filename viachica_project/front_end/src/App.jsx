import { useState, useEffect } from "react";
// import { configApi } from "./config/ConfigApi";
import "./css/task.css";
import axios from "axios";

//

//
const App = () => {
  //const fechaHoraActual = new Date();
  //const fechaHoraEnFormatoISO = fechaHoraActual.toISOString();

  const [tasks, setTasks] = useState({});
  const [add, setAdd] = useState(false);
  const [update, setUpdate] = useState(false);
  const [tarea, setTarea] = useState({
    idTarea: 0,
    nombre: "",
    descripcion: "",
    fechaVencimiento: "",
    prioridad: 0,
    estado: "",
  });
  //
  const updateTask = async () => {
    try {
      await axios.put("https://localhost:7055/api/Tarea", tarea);
      alert("exito");
      getTask();
      setAdd(false);
      setUpdate(false);
    } catch (error) {
      console.error("Error en la solicitud GET", error);
    }
  };

  const obtenerTarea = (task) => {
    setTarea(task);
    setUpdate(true);
    setAdd(true);
  };
  //

  const obtenerTask = (e) => {
    const { name, value } = e.target;
    setTarea({
      ...tarea,
      [name]: value,
    });
  };

  const getTask = async () => {
    try {
      const { data } = await axios.get("https://localhost:7055/api/Tarea");
      setTasks(data);
      // console.log(tasks);
    } catch (error) {
      console.error("Error en la solicitud GET", error);
    }
  };

  const validarDatos = (e) => {
    e.preventDefault();
    if (
      tarea.nombre === "" ||
      tarea.descripcion === "" ||
      tarea.estado === ""
    ) {
      alert("aun hay campos vacios");
    } else {
      if (update) {
        updateTask();
      } else {
        enviarTask();
      }
    }
  };

  const enviarTask = async () => {
    try {
      const response = await axios.post(
        "https://localhost:7055/api/Tarea",
        tarea
      );
      alert(response.data.mensaje);
      getTask();
      setAdd(false);
    } catch (error) {
      console.error("Error en la solicitud GET", error);
    }
  };

  const deleteTask = async (id) => {
    try {
      console.log(id);
      const response = await axios.delete(
        `https://localhost:7055/api/Tarea/${id}`,
        {
          params: {
            IDTarea: id,
          },
        }
      );
      alert(response.data);
      getTask();
    } catch (error) {
      console.error("Error en la solicitud GET", error);
    }
  };

  useEffect(() => {
    getTask();
  }, []);

  if (add) {
    return (
      <div className="container cont-task">
        <div className="cont-form">
          <h2>{update ? "Actualizar " : "Agregar"} Tarea</h2>
          <form className="row" onSubmit={validarDatos}>
            <div className="col-md-2">
              <label htmlFor="idTarea">Ingrese idTarea:</label>
              <input
                className="form-control"
                type="number"
                name="idTarea"
                value={tarea.idTarea}
                onChange={obtenerTask}
              />
            </div>
            <div className="col-md-2">
              <label htmlFor="nombre">Ingrese Nombre:</label>
              <input
                className="form-control"
                type="text"
                name="nombre"
                value={tarea.nombre}
                onChange={obtenerTask}
              />
            </div>
            <div className="col-md-2">
              <label htmlFor="prioridad">
                Ingrese Priorida: {tarea.prioridad}
              </label>
              <input
                className="form-control"
                type="range"
                name="prioridad"
                onChange={obtenerTask}
                max={100}
                min={0}
                value={tarea.prioridad}
              />
            </div>
            <div className="col-md-3">
              <label htmlFor="fechaVencimiento">
                Ingrese Fecha Vencimiento
              </label>
              <input
                className="form-control"
                type="Date"
                name="fechaVencimiento"
                onChange={obtenerTask}
                value={tarea.fechaVencimiento}
              />
            </div>
            <div className="col-md-3">
              <label htmlFor="estado">Ingrese Estado:</label>
              <input
                className="form-control"
                type="text"
                name="estado"
                value={tarea.estado}
                onChange={obtenerTask}
              />
            </div>
            <div className="col-md-12">
              <label htmlFor="descripcion">Ingrese descripcion:</label>
              <textarea
                id="descripcion"
                name="descripcion"
                value={tarea.descripcion}
                rows="4"
                cols="30"
                className="form-control"
                onChange={obtenerTask}
              ></textarea>
            </div>
            <center>
              <button type="submit" className="btn btn-primary mt-5">
                {update ? "Actualizar" : "Guardar"}
              </button>
            </center>
          </form>
        </div>
      </div>
    );
  } else {
    return (
      <div className="container cont-task">
        <div className="cont-btn">
          <button onClick={() => setAdd(true)} className="btn btn-primary">
            <i className="fa-solid fa-circle-plus"></i>
          </button>
        </div>
        <h2>Gestor de Tareas</h2>
        <table className="table table-bordered">
          <thead>
            <tr className="table-head">
              <th>N#</th>
              <th>Nombre</th>
              <th>Descripcion</th>
              <th>FechaVencimiento</th>
              <th>Prioridad</th>
              <th>Estado</th>
              <th colSpan={"2"}>Acciones</th>
            </tr>
          </thead>
          <tbody className="table-body">
            {tasks.length > 0 ? (
              tasks.map((task, i) => (
                <tr key={task.idTarea}>
                  <td>{i + 1}</td>
                  <td>{task.nombre}</td>
                  <td>{task.descripcion}</td>
                  <td>{task.fechaVencimiento}</td>
                  <td>{task.prioridad}</td>
                  <td>{task.estado}</td>
                  <td>
                    <button
                      onClick={() => obtenerTarea(task)}
                      className="btn btn-success"
                    >
                      <i className="fa-solid fa-pencil"></i>
                    </button>
                  </td>

                  <td>
                    <button
                      onClick={() => deleteTask(task.idTarea)}
                      className="btn btn-danger"
                    >
                      <i className="fa-solid fa-trash"></i>
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr></tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }
};

export default App;
