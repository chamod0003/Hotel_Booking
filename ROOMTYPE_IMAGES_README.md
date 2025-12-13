# Room Type Image Management - Implementation Guide

## Overview
This implementation adds the ability to manage images for room types in your hotel booking system. You can now add, update, and retrieve multiple pictures for each room type (e.g., Deluxe, Suite, Standard).

## What Was Added

### 1. Entity Model
- **RoomTypePicture.cs** - New entity to store room type images with properties:
  - `RoomTypePictureId` - Primary key
  - `RoomTypeId` - Foreign key to RoomType
  - `ImageUrl` - URL or path to the image
  - `IsMainPicture` - Flag to mark the main display picture
  - `DisplayOrder` - Order for displaying multiple pictures
  - `Caption` - Optional description for the picture

### 2. Updated RoomType Entity
- Added navigation property: `ICollection<RoomTypePicture> RoomTypePictures`

### 3. Data Transfer Objects (DTOs)
- **CreateRoomTypeDto** - For creating new room types with pictures
- **UpdateRoomTypeDto** - For updating existing room types with pictures
- **CreateRoomTypePictureDto** - For managing picture data

### 4. Repository Layer
- **IRoomTypeRepository** - Interface defining room type data operations
- **RoomTypeRepository** - Implementation with full CRUD operations including picture management

### 5. Service Layer
- **IRoomTypeService** - Interface defining room type business logic
- **RoomTypeService** - Implementation with validation and mapping logic

### 6. API Controller
- **RoomTypeController** - RESTful API endpoints for room type management

### 7. Database Configuration
- Updated **AppDbContext** with RoomType and RoomTypePicture DbSets
- Database migration already includes the RoomTypePictures table

## API Endpoints

### Get All Room Types with Pictures
```
GET /api/roomtype
```
Response example:
```json
[
  {
    "roomId": 1,
    "roomTypeName": "Deluxe Suite",
    "pictures": [
      {
        "imageUrl": "https://example.com/images/deluxe-1.jpg",
        "isMainPicture": true,
        "displayOrder": 1,
        "caption": "Main view of Deluxe Suite"
      },
      {
        "imageUrl": "https://example.com/images/deluxe-2.jpg",
        "isMainPicture": false,
        "displayOrder": 2,
        "caption": "Bathroom view"
      }
    ]
  }
]
```

### Get Room Type by ID
```
GET /api/roomtype/{id}
```

### Create New Room Type with Pictures
```
POST /api/roomtype
Content-Type: application/json
```
Request body:
```json
{
  "roomTypeName": "Presidential Suite",
  "pictures": [
    {
      "imageUrl": "https://example.com/images/presidential-main.jpg",
      "isMainPicture": true,
      "displayOrder": 1,
      "caption": "Presidential Suite Main View"
    },
    {
      "imageUrl": "https://example.com/images/presidential-bathroom.jpg",
      "isMainPicture": false,
      "displayOrder": 2,
      "caption": "Luxury Bathroom"
    }
  ]
}
```

### Update Room Type
```
PUT /api/roomtype/{id}
Content-Type: application/json
```
Request body:
```json
{
  "roomId": 1,
  "roomTypeName": "Deluxe Suite Updated",
  "pictures": [
    {
      "imageUrl": "https://example.com/images/deluxe-new.jpg",
      "isMainPicture": true,
      "displayOrder": 1,
      "caption": "Updated view"
    }
  ]
}
```

### Delete Room Type
```
DELETE /api/roomtype/{id}
```

## Database Migration

The database schema has already been updated in the latest migration. If you need to apply the migration to your database, run:

```bash
dotnet ef database update --project Infrastructure_Layer --startup-project Presentation_Layer
```

## Usage in Frontend

### Example: Displaying Room Type Images
```javascript
// Fetch room types with images
fetch('http://localhost:5000/api/roomtype')
  .then(response => response.json())
  .then(roomTypes => {
    roomTypes.forEach(roomType => {
      console.log(`Room Type: ${roomType.roomTypeName}`);
      
      // Get main picture
      const mainPicture = roomType.pictures.find(p => p.isMainPicture);
      if (mainPicture) {
        console.log(`Main Image: ${mainPicture.imageUrl}`);
      }
      
      // Display all pictures in order
      const sortedPictures = roomType.pictures.sort((a, b) => a.displayOrder - b.displayOrder);
      sortedPictures.forEach(picture => {
        console.log(`- ${picture.caption}: ${picture.imageUrl}`);
      });
    });
  });
```

### Example: Creating Room Type with Images
```javascript
const newRoomType = {
  roomTypeName: "Ocean View Suite",
  pictures: [
    {
      imageUrl: "https://example.com/ocean-view-main.jpg",
      isMainPicture: true,
      displayOrder: 1,
      caption: "Ocean View from Balcony"
    },
    {
      imageUrl: "https://example.com/ocean-view-bedroom.jpg",
      isMainPicture: false,
      displayOrder: 2,
      caption: "King Size Bed"
    }
  ]
};

fetch('http://localhost:5000/api/roomtype', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(newRoomType)
})
  .then(response => response.json())
  .then(data => console.log('Created:', data));
```

## Integration with Hotel Room Management

The room types with pictures can be used when creating or displaying hotel rooms. The existing `HotelRoom` entity already has a relationship with `RoomType`:

```csharp
public class HotelRoom
{
    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; }
    // ... other properties
}
```

When fetching hotel details, you can include room type pictures by modifying the repository query to include:
```csharp
.Include(h => h.Rooms)
    .ThenInclude(r => r.RoomType)
    .ThenInclude(rt => rt.RoomTypePictures)
```

## Best Practices

1. **Main Picture**: Always mark one picture as the main picture (`isMainPicture: true`) for each room type
2. **Display Order**: Use consistent ordering (1, 2, 3...) for predictable display
3. **Image URLs**: Store full URLs or relative paths that your frontend can resolve
4. **Captions**: Add meaningful captions for accessibility and SEO
5. **Image Size**: Consider implementing image optimization on the backend or use a CDN

## Testing the API

You can test the API using Swagger UI (available at `https://localhost:{port}/swagger`) or tools like Postman:

1. Start the application
2. Navigate to the Swagger UI
3. Test the RoomType endpoints
4. Verify that pictures are correctly associated with room types

## Next Steps

Consider implementing:
- Image upload endpoint for handling file uploads
- Image validation (size, format)
- Image storage service (Azure Blob Storage, AWS S3, etc.)
- Image resizing/optimization
- Caching for frequently accessed room type images
