using System.Collections.Generic;

[System.Serializable]
public class IngredienteData
{
    public string nombre;
    public int valor;
    public string iconoId;
}

[System.Serializable]
public class ObjetivoReceta
{
    public string ingrediente;
    public int cantidad;
}

[System.Serializable]
public class RecetaData
{
    public int id;
    public string nombre;
    public List<ObjetivoReceta> objetivos;
}

[System.Serializable]
public class GameData
{
    public List<IngredienteData> ingredientes;
    public List<RecetaData> recetas;
}