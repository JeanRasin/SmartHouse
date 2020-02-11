import { GoalServiceTest } from "./http.goal.test";
import { LoggerServiceTest } from "./http.logger.test";
import { WeatherServiceTest } from "./http.weather.test";

describe('Service tests', () => {
    GoalServiceTest.test();
    LoggerServiceTest.test();
    WeatherServiceTest.test();
});