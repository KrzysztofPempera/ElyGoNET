package main

import (
	"encoding/json"
	"math/rand"
	"net/http"
)

type Response struct {
	Number int `json:"number"`
}

func main() {
	http.HandleFunc("/random", func(w http.ResponseWriter, r *http.Request) {
		randomNumber := rand.Intn(100)

		response := Response{
			Number: randomNumber,
		}

		jsonResponse, err := json.Marshal(response)
		if err != nil {
			http.Error(w, "Internal Server Error", http.StatusInternalServerError)
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.Write(jsonResponse)
	})

	http.ListenAndServe(":8080", nil)
}
