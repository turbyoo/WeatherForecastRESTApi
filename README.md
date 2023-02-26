
# WeatherForecastRESTApi



## API Reference

#### Get three day average forecast for selected city

```http
  GET /api/WeatherForecastAPI/ThreeDaysAverageForecast
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `city` | `string` | **Required**. Selected city |
| `login` | `string` | **Required**. Login |
| `password` | `string` | **Required**. Password |




#### Post weather data to json file.
```http
  POST /api/WeatherForecastAPI/InsertWeatherInfo
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `login`      | `string` | **Required**. Login |
| `password`      | `string` | **Required**. Password |
| `city`      | `string` | **Required**. Selected city |
| `date`      | `string` | **Required**. Selected date |
| `temperature`      | `string` | **Required**. Selected temperature |
| `pressure`      | `string` | **Required**. Selected pressure |
| `windSpeed`      | `string` | **Required**. Selected wind speed |


#### Get weather info for selected city

```http
  GET /api/WeatherForecastAPI/GetWeatherInfo
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `city`      | `string` | **Required**. Selected city |
| `date`      | `string` | **Required**. Selected date |
| `login`      | `string` | **Required**. Login |
| `password`      | `string` | **Required**. Password |


#### Get weather info for selected cities

```http
  POST /api/WeatherForecastAPI/CityForecast
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `login`      | `string` | **Required**. Login |
| `password`      | `string` | **Required**. Password |
| `city`      | `string` | **Required**. Selected city name |


Example: "Poznan", "New York", "Dublin"



