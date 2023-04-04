﻿using Dapper;
using PioresFilmes.Data.Interfaces;
using PioresFilmes.Domain;
using System.Data;

namespace PioresFilmes.Data
{
    public class ProducerRepository: IProducerRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProducerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Producer> FindProducerWithLeastIntervalBetweenWins()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Producer>(@"
                SELECT Producers.Name, Producers.Id, MIN(Current.WinningYear - Previous.WinningYear) as YearsBetweenWins
                FROM (
                    SELECT 
                        mp1.ProducerId, 
                        m1.Year as WinningYear, 
                        ROW_NUMBER() OVER(PARTITION BY mp1.ProducerId ORDER BY m1.Year ASC) as WinNumber
                    FROM MovieProducers mp1
                    INNER JOIN Movies m1 ON mp1.MovieId = m1.Id
                    WHERE m1.Winner = 1
                ) as Current
                INNER JOIN (
                    SELECT 
                        mp2.ProducerId, 
                        m2.Year as WinningYear, 
                        ROW_NUMBER() OVER(PARTITION BY mp2.ProducerId ORDER BY m2.Year ASC) as WinNumber
                    FROM MovieProducers mp2
                    INNER JOIN Movies m2 ON mp2.MovieId = m2.Id
                    WHERE m2.Winner = 1
                ) as Previous ON Current.ProducerId = Previous.ProducerId AND Current.WinNumber = Previous.WinNumber + 1
                INNER JOIN Producers ON Producers.Id = Current.ProducerId
                GROUP BY Producers.Name
                HAVING COUNT(*) >= 1
                ORDER BY YearsBetweenWins ASC
                LIMIT 1"
            );
            return result;
        }

        public async Task<Producer> FindProducerWithGreatestIntervalBetweenWins()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Producer>(@"
                SELECT Producers.Name, Producers.Id, MIN(Current.WinningYear - Previous.WinningYear) as YearsBetweenWins
                FROM (
                    SELECT 
                        mp1.ProducerId, 
                        m1.Year as WinningYear, 
                        ROW_NUMBER() OVER(PARTITION BY mp1.ProducerId ORDER BY m1.Year ASC) as WinNumber
                    FROM MovieProducers mp1
                    INNER JOIN Movies m1 ON mp1.MovieId = m1.Id
                    WHERE m1.Winner = 1
                ) as Current
                INNER JOIN (
                    SELECT 
                        mp2.ProducerId, 
                        m2.Year as WinningYear, 
                        ROW_NUMBER() OVER(PARTITION BY mp2.ProducerId ORDER BY m2.Year ASC) as WinNumber
                    FROM MovieProducers mp2
                    INNER JOIN Movies m2 ON mp2.MovieId = m2.Id
                    WHERE m2.Winner = 1
                ) as Previous ON Current.ProducerId = Previous.ProducerId AND Current.WinNumber = Previous.WinNumber + 1
                INNER JOIN Producers ON Producers.Id = Current.ProducerId
                GROUP BY Producers.Name
                HAVING COUNT(*) >= 1
                ORDER BY YearsBetweenWins DESC
                LIMIT 1"
            );
            return result;
        }
    }
}